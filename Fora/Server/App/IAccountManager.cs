
namespace Fora.Server.App
{
    public interface IAccountManager
    {
        Task UpdateUserInAuthDb(ApplicationUser newUser);
        string GenerateToken();

        Task AddUserToForaDb(UserDTOModel dtoModel);

    }
}