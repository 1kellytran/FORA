
using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IDataManager
    {
        Task CreateIntrest(InterestModel interestToAdd);
        Task<List<InterestModel>> GetAllInterests();
    }
}
