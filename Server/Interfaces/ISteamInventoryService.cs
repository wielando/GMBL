using GMBL.Server.Services;
using GMBL.Shared;

namespace GMBL.Server.Interfaces
{
    public interface ISteamInventoryService
    {
        public Task<List<SteamInventoryDto>> GetCSGOItemsFromSteamInventory(string steamId);
    }
}
