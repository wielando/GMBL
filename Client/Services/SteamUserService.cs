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
    }

}
