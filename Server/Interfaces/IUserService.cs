namespace GMBL.Server.Interfaces
{
    public interface IUserService
    {
        Task UpdateUserWithSteamData(string userId, string steamProfileName, string steamProfileImageUrl);
        Task CreateOrUpdateUser(string steamId, string steamProfileName, string steamProfileImageUrl);


    }
}
