using GMBL.Server.Helper;
using GMBL.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GMBL.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISteamAuthService _steamAuthService;

        public UserController(ISteamAuthService steamAuthService)
        {
            _steamAuthService = steamAuthService;
        }

        [HttpGet("userinfo")]
        [Authorize]
        public async Task<ActionResult<SteamUserInfo>> GetSteamUserInfo()
        {
            var steamId = User.FindFirstValue("sub");

            // Überprüfen, ob der Nutzer mit dem Steamkonto übereinstimmt
            if (!await _steamAuthService.ValidateSteamUser(steamId))
            {
                return Unauthorized();
            }

            // Hier können Sie weitere Überprüfungen oder Aktionen durchführen, die für den eingeloggten Nutzer relevant sind

            // Beispiel: Annahme, dass der Steam-Benutzername im Claim "name" vorhanden ist
            var steamUserName = User.FindFirstValue("name");

            // Beispiel: Annahme, dass der Steam-Benutzeravatar im Claim "avatar" vorhanden ist
            var steamUserAvatar = User.FindFirstValue("avatar");

            var steamUserInfo = new SteamUserInfo
            {
                UserName = steamUserName,
                UserAvatar = steamUserAvatar
            };

            return steamUserInfo;
        }
    }
}
