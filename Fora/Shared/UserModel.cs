namespace Fora.Shared
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public bool Banned { get; set; }
        public bool Deleted { get; set; }
        public List<UserInterestModel> UserInterests { get; set; } = new();// Interests this user has
        public List<InterestModel> Interests { get; set; } = new();// Interests created by this user
        public List<ThreadModel> Threads { get; set; } = new();// Threads created by this user
        public List<MessageModel> Messages { get; set; } = new();// Messages created by this user
    }
}
