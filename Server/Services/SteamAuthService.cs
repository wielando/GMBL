using GMBL.Server.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GMBL.Server.Services
{
    public class SteamAuthService : ISteamAuthService
    {
        private readonly HttpClient _httpClient;

        public SteamAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidateSteamUser(string steamId)
        {
            // Senden Sie eine Anfrage an die Steam Web API, um die Profildaten des Benutzers abzurufen
            var response = await _httpClient.GetAsync($"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=0544B7ECC0E787AE36B64B5907113FAE&steamids={steamId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Überprüfen Sie, ob die Antwort Daten enthält und ob der Benutzer erfolgreich abgerufen wurde
                var responseData = JObject.Parse(content);
                var players = responseData["response"]["players"];

                if (players != null && players.HasValues)
                {
                    // Überprüfen Sie, ob der Spieler erfolgreich abgerufen wurde
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

    }
}