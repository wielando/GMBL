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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(ISteamAuthService steamAuthService, IHttpContextAccessor httpContextAccessor)
        {
            _steamAuthService = steamAuthService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("steam-user-id")]
        public IActionResult GetSteamUserId()
        {
            var steamUserId = _httpContextAccessor.HttpContext.Session.GetString("SteamUserId");

            return Ok(steamUserId);
        }

    }
}
