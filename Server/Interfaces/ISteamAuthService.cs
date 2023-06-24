using Microsoft.AspNetCore.Mvc;

namespace GMBL.Server.Interfaces
{
    public interface ISteamAuthService
    {
        public Task<bool> ValidateSteamUser(string steamId);

        Task<string> GetSteamProfileName(string steamId);

        Task<string> GetSteamProfileImageUrl(string steamId);
    }
}
