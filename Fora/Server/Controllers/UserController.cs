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
        public async Task<ActionResult<string>> SignUpUser([FromBody] UserDTOModel userToSignUp)
        {
            // ***** REGISTER USER *****

            // Create empty, new user
            ApplicationUser newUser = new();

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

                // Send that token back
                return Ok(token);
            }
            return BadRequest("Could not create user");
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult> SignInUser(SignInModel userToSignIn)
        {
            var applicationUser = await _signInManager.UserManager.FindByNameAsync(userToSignIn.Username);

            if(applicationUser != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(applicationUser, userToSignIn.Password, false);

                if (signInResult.Succeeded)
                {
                    //Generate token
                    string token = _accountManager.GenerateToken();

                    //Send token back
                    applicationUser.Token = token;

                    // Add the new token (update) in the identity db
                    await  _accountManager.UpdateUserInAuthDb(applicationUser);

                    return Ok(token);
                }
            }
            return BadRequest("User not found");
        }

        // GET: api/<UserController>
        [HttpGet("{id}")]
        public ActionResult<UserModel> GetUserById(int id)
        {
            UserModel user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return BadRequest("404 - User not found");
            }
            return Ok(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<UserController>/5
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
    }
}
