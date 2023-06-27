using GMBL.Server.Interfaces;
using GMBL.Shared;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GMBL.Server.Services
{

    public class SteamInventoryService : ISteamInventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public SteamInventoryService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
        }

        public async Task<List<SteamInventoryDto>> GetCSGOItemsFromSteamInventory(string steamId)
        {
            var steamIds = "76561198111950965";

            var response = await _httpClient.GetAsync($"https://steamcommunity.com/inventory/{steamIds}/730/2?l=english&count=5000");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var inventoryResponse = JsonConvert.DeserializeObject<JObject>(responseContent);

                var descriptions = inventoryResponse.SelectToken("descriptions")?.ToObject<JArray>();
                if (descriptions != null)
                {
                    var steamInventoryItems = new List<SteamInventoryDto>();

                    foreach (var description in descriptions)
                    {
                        {
                            var marketName = description.SelectToken("market_name")?.Value<string>();
                            var name = description.SelectToken("name")?.Value<string>();
                            var iconUrl = description.SelectToken("icon_url")?.Value<string>();
                            var iconUrlLarge = description.SelectToken("icon_url_large")?.Value<string>();

                            if (marketName != null && name != null && iconUrl != null && iconUrlLarge != null)
                            {
                                var steamInventoryItem = new SteamInventoryDto
                                {
                                    marketName = marketName,
                                    name = name,
                                    iconUrl = iconUrl,
                                    iconUrlLarge = iconUrlLarge
                                };

                                steamInventoryItems.Add(steamInventoryItem);
                            }
                        }
                    }


                    return steamInventoryItems;
                }
            }

            // Handle error response
            // ...

            return null;
        }



    }

    public class SteamInventoryResponse
    {

        public string iconUrl { get; set; }
        public string iconUrlLarge { get; set; }
        public string name { get; set; }
        public string marketName { get; set; }

    }

    public class SteamInventoryResult
    {
        public List<SteamInventoryDto> Items { get; set; }
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
