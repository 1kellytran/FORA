using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IUserManager
    {
        Task<UserModel> GetUserById(int id);
        Task<string> SignUpUser(UserDTOModel user);

        Task<string> SignInUser(SignInModel user);

        Task DeleteUser(int id);
    }
}
