using GMBL.Server.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GMBL.Server.Services 
{
    public class SteamInventoryItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ImageId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class SteamInventoryService : ISteamInventoryService
    {
        private readonly HttpClient _httpClient;

        public SteamInventoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SteamInventoryItem>> GetCSGOItemsFromSteamInventory()
        {
            var response = await _httpClient.GetAsync("https://api.steampowered.com/IEconItems_730/GetPlayerItems/v1?key=YOUR_API_KEY&steamid=YOUR_STEAMID");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var inventoryResponse = JsonSerializer.Deserialize<SteamInventoryResponse>(responseContent);

                var csgoItems = new List<SteamInventoryItem>();
                foreach (var item in inventoryResponse.Result.Items)
                {
                    if (item.Type == "CSGOType")
                    {
                        var imageUrl = await GetItemImageUrl(item.ImageId);

                        csgoItems.Add(new SteamInventoryItem
                        {
                            Name = item.Name,
                            Type = item.Type,
                            ImageUrl = imageUrl
                        });
                    }
                }

                return csgoItems;
            }

            // Handle error response
            // ...

            return null;
        }

        private async Task<string> GetItemImageUrl(string imageId)
        {
            var response = await _httpClient.GetAsync($"https://api.steampowered.com/IEconItems_730/GetItemIconPath/v1?key=YOUR_API_KEY&iconname={imageId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var imageResponse = JsonSerializer.Deserialize<SteamImageResponse>(responseContent);
                return imageResponse.Result.Path;
            }

            // Handle error response
            // ...

            return null;
        }
    }

    public class SteamInventoryResponse
    {
        public SteamInventoryResult Result { get; set; }
    }

    public class SteamInventoryResult
    {
        public List<SteamInventoryItem> Items { get; set; }
    }

    public class SteamImageResponse
    {
        public SteamImageResult Result { get; set; }
    }

    public class SteamImageResult
    {
        public string Path { get; set; }
    }
}
