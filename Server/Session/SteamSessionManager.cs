    namespace GMBL.Server.Session
    {
        using Microsoft.AspNetCore.Http;

        public class SteamSessionManager
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private const string SteamUserIdKey = "SteamUserId";

            public SteamSessionManager(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public void SetAuthenticatedUser(string steamUserId)
            {
                _httpContextAccessor.HttpContext.Session.SetString(SteamUserIdKey, steamUserId);
            }

            public string GetAuthenticatedUser()
            {
                return _httpContextAccessor.HttpContext.Session.GetString(SteamUserIdKey);
            }

            public bool IsUserAuthenticated()
            {
                return !string.IsNullOrEmpty(GetAuthenticatedUser());
            }

            public void ClearAuthenticatedUser()
            {
                _httpContextAccessor.HttpContext.Session.Remove(SteamUserIdKey);
            }
        }

    }
