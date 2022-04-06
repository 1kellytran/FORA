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
        private readonly AuthDbContext _authContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountManager _accountManager;


        public UserController(AppDbContext context, /*AuthDbContext authContext,*/ SignInManager<ApplicationUser> signInManager, IAccountManager accountManager)

        {
            _context = context;
            _authContext = authContext;
            _signInManager = signInManager;
            _accountManager = accountManager;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<string>> SignInAsync([FromBody] UserDTOModel userToSignIn)
        {
            return Ok("");
        }

        [HttpPost]
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

                //give user token
                newUser.Token = token;
                await _accountManager.UpdateUserInAuthDb(newUser);

                // Send that token back
                return Ok(token);
            }

            return BadRequest("Couldn't create user");
        }



        // GET: api/<UserController>
        [HttpGet("{id}")]
        public ActionResult<UserModel> GetUser(int id)
        {
            UserModel user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return BadRequest("404 - User not found");
            }
            return Ok(user);
        }


        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            UserModel user = _context.Users.FirstOrDefault(x => x.Id == id);


        }
    }
}
