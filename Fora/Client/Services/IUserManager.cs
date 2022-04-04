using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IUserManager
    {
        Task<UserModel> GetUser(int id);
    }
}