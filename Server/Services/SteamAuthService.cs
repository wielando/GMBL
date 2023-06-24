using GMBL.Server.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GMBL.Server.Services
{
    public class SteamAuthService : ISteamAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        private readonly string API_KEY = "";

    

        public SteamAuthService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings.Value;

            API_KEY = _appSettings.SteamApiKey;
        }

        public async Task<bool> ValidateSteamUser(string steamId)
        {
            var response = await _httpClient.GetAsync($"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={API_KEY}&steamids={steamId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Check that the response contains data and that the user was retrieved successfully
                var responseData = JObject.Parse(content);
                var players = responseData["response"]["players"];

                if (players != null && players.HasValues)
                {
                    // Check if the profile was retrieved successfully
                    var player = players.First;
                    var playerSteamId = player["steamid"].ToString();

                    if (playerSteamId == steamId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<string> GetSteamProfileName(string steamId)
        {

            if (!ValidateSteamUser(steamId).Result) return "Wrong user!";

            var apiKey = API_KEY;
            var apiUrl = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={apiKey}&steamids={steamId}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JObject.Parse(content);

                    var players = result["response"]["players"];
                    var player = players.FirstOrDefault();
                    if (player != null)
                    {
                        var steamProfileName = player["personaname"].ToString();
                        return steamProfileName;
                    }
                }
            }

            // Fallback value in case the API call fails
            return "Steam Profile Name";
        }

        public async Task<string> GetSteamProfileImageUrl(string steamId)
        {
            if (!ValidateSteamUser(steamId).Result) return "Wrong user!";

            var apiKey = API_KEY;
            var apiUrl = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={apiKey}&steamids={steamId}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JObject.Parse(content);

                    var players = result["response"]["players"];
                    var player = players.FirstOrDefault();
                    if (player != null)
                    {
                        var steamProfileImageUrl = player["avatarfull"].ToString();
                        return steamProfileImageUrl;
                    }
                }
            }

            // Fallback value in case the API call fails
            return "Steam Profile Image URL";
        }

    }
}