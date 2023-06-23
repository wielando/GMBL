using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GMBL.Server.Controllers
{
    [Route("/auth")]
    public class AuthentificationController : Controller
    {

        [HttpGet("steam-login")]
        public IActionResult SteamLogin()
        {
            var redirectUrl = Url.Action("SteamCallback", "SteamAuth", null, Request.Scheme);
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("steam-callback")]
        public async Task<IActionResult> SteamCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (authenticateResult.Succeeded)
            {
  
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LoginFailed", "Home");
            }
        }
    }
}
