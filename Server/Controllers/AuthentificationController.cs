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

namespace GMBL.Server.Controllers
{

    [Route("/auth")]
    public class AuthentificationController : Controller
    {

        private readonly IUserService _userService;
        private readonly ISteamAuthService _steamAuthService;

        public AuthentificationController(IUserService userService, ISteamAuthService steamAuthService)
        {
            _userService = userService;
            _steamAuthService = steamAuthService;
        }

        [HttpGet("steam-login")]
        public IActionResult SteamLogin()
        {
            var redirectUrl = Url.Action(nameof(SteamCallback), "SteamAuth", null, Request.Scheme);
            var openIdUrl = "https://steamcommunity.com/openid/login";
            var returnUrl = Request.Scheme + "://" + Request.Host.Value + Url.Action(nameof(SteamCallback), "Authentification");

            var parameters = new Dictionary<string, string>
            {
                { "openid.ns", "http://specs.openid.net/auth/2.0" },
                { "openid.mode", "checkid_setup" },
                { "openid.identity", "http://specs.openid.net/auth/2.0/identifier_select" },
                { "openid.claimed_id", "http://specs.openid.net/auth/2.0/identifier_select" },
                { "openid.return_to", returnUrl },
                { "openid.realm", Request.Scheme + "://" + Request.Host.Value }
            };

            var queryString = string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value ?? "")}"));
            var redirectUri = $"{openIdUrl}?{queryString}";

            return Redirect(redirectUri);
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

                // Call userdata from steamprofile
                var steamProfileName = _steamAuthService.GetSteamProfileName(steamId);
                var steamProfileImageUrl = _steamAuthService.GetSteamProfileImageUrl(steamId);

                // Call UserService to update the data in the database
                await _userService.CreateOrUpdateUser(steamId, await steamProfileName, await steamProfileImageUrl);

                return Ok($"Eingeloggt als Steam ID: {steamId}");
            }

            // Steam-Login failed
            return RedirectToAction("LoginFailed");
        }

    }
}
