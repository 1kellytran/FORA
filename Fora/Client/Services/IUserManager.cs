using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IUserManager
    {
        Task<UserModel> GetUserToken(string token);
        Task<List<string>> SignUpUser(UserDTOModel user);

        Task<List<string>> SignInUser(SignInModel user);

        Task DeleteUser(int id);

        Task<UserStatusDTOModel> CheckUserLogin(string token);
    }
}
