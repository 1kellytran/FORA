
namespace Fora.Server.App
{
    public interface IAccountManager
    {
        Task UpdateUserInAuthDb(ApplicationUser newUser);
        string GenerateToken();
        void ChangePassword(ApplicationUser userToChange, string password);
    }
}