using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IDataManager
    {
        Task<List<InterestModel>> GetAllUsers();
    }
}
