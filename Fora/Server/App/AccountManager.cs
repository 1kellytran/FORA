using Microsoft.AspNetCore.Identity;

namespace Fora.Server.App
{
    public class AccountManager : IAccountManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        

        public AccountManager(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            
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
    }
}
