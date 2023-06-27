using GMBL.Server.Helper;
using GMBL.Server.Interfaces;
using GMBL.Server.Services;
using GMBL.Shared;
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
        private readonly ISteamInventoryService _inventoryService;

        public UserController(ISteamAuthService steamAuthService, IHttpContextAccessor httpContextAccessor, ISteamInventoryService inventoryService)
        {
            _steamAuthService = steamAuthService;
            _httpContextAccessor = httpContextAccessor;
            _inventoryService = inventoryService;
        }

        [HttpGet("steam-user-id")]
        public IActionResult GetSteamUserId()
        {
            var steamUserId = _httpContextAccessor.HttpContext.Session.GetString("SteamUserId");

            return Ok(steamUserId);
        }

        [HttpGet("steam-user-inventory")]
        public async Task<List<SteamInventoryDto>> GetSteamInventory()
        {
            var steamUserId = _httpContextAccessor.HttpContext.Session.GetString("SteamUserId");

            var t = await _inventoryService.GetCSGOItemsFromSteamInventory(steamUserId);

            return await _inventoryService.GetCSGOItemsFromSteamInventory(steamUserId);
        }

    }
}
