using GMBL.Server.Services;

namespace GMBL.Server.Interfaces
{
    public interface ISteamInventoryService
    {
        public Task<List<SteamInventoryItem>> GetCSGOItemsFromSteamInventory();
    }
}
