using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IDataManager
    {
        // ***** INTERESTS *****
        Task CreateInterest(InterestModel interestToAdd);
        Task<List<InterestModel>> GetAllInterests();
        Task<string> DeleteInterest(int id);

        Task<List<InterestModel>> GetUserInterests(int activeUserId);

        Task CreateNewUserInterest(UserInterestModel UserInterestToAdd);


        // ***** THREADS *****
        Task<List<ThreadModel>> GetAllThreads(int interestID);
        Task CreateThread(ThreadModel threadToAdd);


        // ***** MESSAGES *****
        Task<List<MessageModel>> GetAllMessages(int threadID);
        Task CreateMessage(MessageModel messageToAdd);
        Task<ThreadModel> GetThreadById(int id);
    }
}
