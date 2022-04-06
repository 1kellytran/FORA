using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IUserManager
    {
        Task<UserModel> GetUser(int id);
        Task<string> SignUpUser(UserDTOModel user);

        Task<string> SignInUser(UserDTOModel user);
    }
}
