using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using GMBL.Server.Helper;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using GMBL.Server.Services;
using Microsoft.EntityFrameworkCore;
using GMBL.Server.Interfaces;
using Microsoft.Extensions.Options;
using GMBL.Server.Session;

namespace GMBL.Server.Controllers
{

    [Route("/auth")]
    public class AuthentificationController : Controller
    {

        private readonly IUserService _userService;
        private readonly ISteamAuthService _steamAuthService;
        private readonly SteamSessionManager _steamSessionManager;

        private readonly AppSettings _appSettings;

        public AuthentificationController(IUserService userService, ISteamAuthService steamAuthService, IOptions<AppSettings> appSettings, SteamSessionManager steamSessionManager)
        {
            _userService = userService;
            _steamAuthService = steamAuthService;
            _appSettings = appSettings.Value;
            _steamSessionManager = steamSessionManager;
        }



        [HttpGet("steam-callback")]
        public async Task<IActionResult> SteamCallback()
        {
            var queryString = HttpContext.Request.QueryString.Value;
            var queryParameters = QueryHelpers.ParseQuery(queryString);

            if (queryParameters.ContainsKey("openid.identity"))
            {
                var claimedId = queryParameters["openid.identity"];
                var steamId = claimedId.First().Split('/').Last();

                if (_steamSessionManager.IsUserAuthenticated())
                {
                    return RedirectToAction("LoggedIn");
                }

                // Call userdata from steamprofile
                var steamProfileName = _steamAuthService.GetSteamProfileName(steamId);
                var steamProfileImageUrl = _steamAuthService.GetSteamProfileImageUrl(steamId);

                // Call UserService to update the data in the database
                await _userService.CreateOrUpdateUser(steamId, await steamProfileName, await steamProfileImageUrl);

                _steamSessionManager.SetAuthenticatedUser(steamId);

                return RedirectToAction("LoggedIn");
            }

            // Steam-Login failed
            return RedirectToAction("LoginFailed");
        }

        [HttpGet("logged-in")]
        public IActionResult LoggedIn()
        {
            // Überprüfen, ob der Benutzer authentifiziert ist
            if (_steamSessionManager.IsUserAuthenticated())
            {
                return Ok("Logged in");
            }

            return RedirectToAction("LoginFailed");
        }

    }
}
