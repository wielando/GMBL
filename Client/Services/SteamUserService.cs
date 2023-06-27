using GMBL.Shared;
using System.Net.Http.Json;
using System.Text.Json;

namespace GMBL.Client.Services
{
    public class SteamUserService
    {
        private readonly HttpClient _httpClient;

        public SteamUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetLoggedInSteamUserId()
        {
            var response = await _httpClient.GetAsync("/api/user/steam-user-id");
            response.EnsureSuccessStatusCode();

            var steamUserId = await response.Content.ReadAsStringAsync();

            return steamUserId;
        }

        public async Task<List<SteamInventoryDto>> GetSteamUserInventory()
        {
            var response = await _httpClient.GetAsync("/api/user/steam-user-inventory");
            if (response.IsSuccessStatusCode)
            {
                var steamInventory = await response.Content.ReadFromJsonAsync<List<SteamInventoryDto>>();
                return steamInventory;
            }

            // Handle error response
            // ...

            return null;
        }   

    }

}
