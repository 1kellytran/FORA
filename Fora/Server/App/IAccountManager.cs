
namespace Fora.Server.App
{
    public interface IAccountManager
    {
        Task UpdateUserInAuthDb(ApplicationUser newUser);
        string GenerateToken();
        Task ChangePassword(ApplicationUser user, PasswordDTOModel userToUpdate);
        Task AddUserToForaDb(UserDTOModel dtoModel);
    }
}