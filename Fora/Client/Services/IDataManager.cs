
using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IDataManager
    {
        Task CreateInterest(InterestModel interestToAdd);
        Task<List<InterestModel>> GetAllInterests();
        Task<string> DeleteInterest(int id);
    }
}
