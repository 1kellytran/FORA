using Microsoft.AspNetCore.Identity;

namespace Fora.Server.App
{
    public class AccountManager : IAccountManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountManager(SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public async Task UpdateUserInAuthDb(ApplicationUser newUser)
        {
            await _signInManager.UserManager.UpdateAsync(newUser);
        }

        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString();
            return token;
        }

        
        public async Task ChangePassword(ApplicationUser user, PasswordDTOModel userToUpdate)
        {
            
            await _signInManager.UserManager.ChangePasswordAsync(user, userToUpdate.OldPassword, userToUpdate.NewPassword);
            
        }

        public async Task AddUserToForaDb(UserDTOModel dtoModel)
        {
            UserModel userToAdd = new();
            userToAdd.Username = dtoModel.Username;
            userToAdd.Deleted = false;
            userToAdd.Banned = false;

            _context.Users.Add(userToAdd);
            _context.SaveChanges();
        }
    }
}



