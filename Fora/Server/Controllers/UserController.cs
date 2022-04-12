using Fora.Server.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthDbContext _authContext; //not neccesary??
        private readonly SignInManager<ApplicationUser> _signInManager; //move to accoutManager?? (all code)
        private readonly IAccountManager _accountManager;

        public UserController(AppDbContext context, AuthDbContext authContext, SignInManager<ApplicationUser> signInManager, IAccountManager accountManager)
        {
            _context = context;
            _authContext = authContext;
            _signInManager = signInManager;
            _accountManager = accountManager;
        }

        [HttpPost]
        public async Task<ActionResult<List<string>>> SignUpUser([FromBody] UserDTOModel userToSignUp)
        {
            // ***** REGISTER USER *****

            // Create empty, new user
            ApplicationUser newUser = new();
            List<string> localStorageInfo = new();

            // Add properties to identity user
            newUser.UserName = userToSignUp.Username;
            newUser.Token = "";

            // Create user
            var createUserResult = await _signInManager.UserManager.CreateAsync(newUser, userToSignUp.Password);

            if (createUserResult.Succeeded)
            {
                // Generate token
                string token = _accountManager.GenerateToken();

                // Give user token
                newUser.Token = token;

                // Update user in authDb
                await _accountManager.UpdateUserInAuthDb(newUser);

                // Add user to Fora database
                await _accountManager.AddUserToForaDb(userToSignUp);

                localStorageInfo.Add(token);
                localStorageInfo.Add(userToSignUp.Username);
                // Send that token back
                return Ok(localStorageInfo);
            }
            return BadRequest("Could not create user");
        }

        [HttpGet]
        [Route("check")]
        public async Task<ActionResult<UserStatusDTOModel>> CheckUserLogin([FromQuery] string accessToken)
        {
            var result = _signInManager.UserManager.Users.FirstOrDefault(x => x.Token == accessToken);

            if (result.Token == accessToken)
            {
                UserStatusDTOModel userStatus = new();
                userStatus.IsLoggedIn = true;
                //userStatus.IsAdmin = await _signInManager.UserManager.IsInRoleAsync(user, "Admin");

                return Ok(userStatus);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<List<string>>> SignInUser(SignInModel userToSignIn)
        {
            var applicationUser = await _signInManager.UserManager.FindByNameAsync(userToSignIn.Username);
            List<string> localStorageInfo = new();

            if (applicationUser != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(applicationUser, userToSignIn.Password, false);

                if (signInResult.Succeeded)
                {
                    //Generate token
                    string token = _accountManager.GenerateToken();

                    //give user token
                    applicationUser.Token = token;

                    // Add the new token (update) in the identity db
                    await _accountManager.UpdateUserInAuthDb(applicationUser);

                    localStorageInfo.Add(token);
                    localStorageInfo.Add(applicationUser.UserName);

                    return Ok(localStorageInfo);
                }
            }
            return BadRequest("User not found");
        }

        //GET: api/<UserController>
        [HttpGet]
        [Route("getbytoken")]
        public async Task<ActionResult<UserModel>> GetUserByToken([FromQuery] string accessToken)
        {
            var userFromAuthDb = _signInManager.UserManager.Users.FirstOrDefault(x => x.Token == accessToken);
            if (userFromAuthDb.Token == accessToken)
            {
                var userFromForaDb = _context.Users.FirstOrDefault(x => x.Username == userFromAuthDb.UserName);
                return userFromForaDb;
            }
            else
            {
                return BadRequest("No user for you my friend");
            }
        }

        //// PUT api/<UserController>/5 test alex
        //[HttpPut]
        //public async Task UpdateUser([FromBody] UserModel updatedUser)
        //{
        //    UserModel userToUpdate = new();
        //    userToUpdate.Id = updatedUser.Id;
        //    userToUpdate.Username = updatedUser.Username;
        //    userToUpdate.UserInterests = updatedUser.UserInterests;
        //    userToUpdate.Interests = updatedUser.Interests;
        //    userToUpdate.Messages = updatedUser.Messages;
        //    userToUpdate.Banned = updatedUser.Banned;
        //    userToUpdate.Deleted = updatedUser.Deleted;
        //    userToUpdate.Threads = updatedUser.Threads;

        //    _context.Users.Update(userToUpdate);
        //    _context.SaveChanges();
        //}

        // DELETE api/<UserController>/5 test alex
        [HttpDelete("{id}")]
        public async Task DeleteUser(int id)
        {
            UserModel user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                _context.Users.Remove(user);
                //var createUserResult = await _signInManager.UserManager.DeleteAsync(); //flytta till AccountManager?
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePassword([FromBody] PasswordDTOModel userToUpdate)
        {

            var applicationUser = await _signInManager.UserManager.FindByNameAsync(userToUpdate.Username);
            if (applicationUser != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(applicationUser, userToUpdate.OldPassword, false);
                if (signInResult.Succeeded)
                {
                    await _accountManager.ChangePassword(applicationUser, userToUpdate);
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}
