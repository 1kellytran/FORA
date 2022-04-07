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
        //[Route("SignUp")]
        public async Task<ActionResult<string>> SignUpAsync([FromBody] UserDTOModel userToSignUp)
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
            return BadRequest("Couldn't create user");
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult> SignInUser(SignInModel userToSignIn)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(userToSignIn.Username, userToSignIn.Password, false, false);

            if (signInResult.Succeeded)
            {
                //Generate token
                string token = _accountManager.GenerateToken();

                //Send token back
                userToSignIn.Token = token;
                return Ok(token);
            }
            return BadRequest("User not found");
        }

        //[HttpPost]
        //[Route("signin")]
        //public async Task<ActionResult> SignInAsync(SignInModel userToSignIn)
        //{
        //    var signInResult = await _signInManager.PasswordSignInAsync(userToSignIn.Username, userToSignIn.Password, false, false);
        //    if (signInResult.Succeeded)
        //    {
        //        string token = _accountManager.GenerateToken();
        //        userToSignIn.Token = token;

        //        return Ok(token);
        //    }
        //    //return BadRequest("User not found.");
        //    return NotFound();
        //}

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
