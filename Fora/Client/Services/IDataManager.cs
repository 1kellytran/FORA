using Fora.Shared;

namespace Fora.Client.Services
{
    public interface IDataManager
    {
        // ***** INTERESTS *****
        Task CreateInterest(InterestModel interestToAdd, string token);
        Task<List<InterestModel>> GetAllInterests();
        Task<string> DeleteInterest(int id);

        Task<List<InterestModel>> GetUserInterests(int activeUserId);

        Task CreateNewUserInterest(UserInterestModel UserInterestToAdd);
        Task DeleteFromInterest(int interestId);
        Task RemoveInterestFromFav(int interestID, int userID);


        // ***** THREADS *****
        Task<List<ThreadModel>> GetAllThreadsById(int interestID);
        Task CreateThread(ThreadModel threadToAdd);
        Task<List<ThreadModel>> GetActiveUserThreds(int userId);
        Task<ThreadModel> GetThreadById(int id);
        Task DeleteThread(int threadToDeleteId);


        // ***** MESSAGES *****
        Task<List<MessageModel>> GetAllMessages(int threadID);
        Task CreateMessage(MessageModel messageToAdd);
        Task DeleteMessage(int messageID);
        Task EditMessage(int messageID, MessageModel messageToEdit);
    }
}
