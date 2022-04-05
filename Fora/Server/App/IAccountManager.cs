
namespace Fora.Server.App
{
    public interface IAccountManager
    {
        Task UpdateUserInDb(ApplicationUser newUser);
        string GenerateToken();
    }
}