using Microsoft.AspNetCore.Identity;

namespace Fora.Server.App
{
    public class UserManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserManager(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task UpdateUserToken(ApplicationUser newUser)
        {
            await _signInManager.UserManager.UpdateAsync(newUser);
        }
    }
}
