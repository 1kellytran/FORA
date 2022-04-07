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

        //in prgress, dont know if it works
        public async void ChangePassword(ApplicationUser userToChange,string password)
        {
            //removes password?
            await _signInManager.UserManager.RemovePasswordAsync(userToChange);
            //adds new password? requirements, add in page?? 
            await _signInManager.UserManager.AddPasswordAsync(userToChange, password);

        }
    }
}
