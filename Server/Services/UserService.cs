using GMBL.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GMBL.Server.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateUserWithSteamData(string steamId, string steamProfileName, string steamProfileImageUrl)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.SteamId == steamId);

            if (user != null)
            {
                user.Username = steamProfileName;
                user.AvatarUrl = steamProfileImageUrl;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task CreateOrUpdateUser(string steamId, string steamProfileName, string steamProfileImageUrl)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.SteamId == steamId);

            if (user == null)
            {
                // Benutzer existiert noch nicht, daher erstellen
                user = new Dto.UserDto
                {
                    SteamId = steamId,
                    Username = steamProfileName,
                    AvatarUrl = steamProfileImageUrl
                };

                _dbContext.Users.Add(user);
            }
            else
            {
                // User already exists, so update
                await UpdateUserWithSteamData(steamId, steamProfileName, steamProfileImageUrl);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
