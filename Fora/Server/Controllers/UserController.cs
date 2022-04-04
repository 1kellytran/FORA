using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthDbContext _authContext;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(AppDbContext context, AuthDbContext authContext, SignInManager<IdentityUser> signInManager) 
        {
            _context = context;
            _authContext = authContext;
            _signInManager = signInManager;
        }
        public async Task SignUpAsync(string username, string password)
        {
            // ***** REGISTER USER *****

            // Create empty, new user
            IdentityUser newUser = new();

            // Add properties to identity user
            newUser.UserName = username;

            // Create user
            var createUserResult = await _signInManager.UserManager.CreateAsync(newUser, password);
        }
        // GET: api/<UserController>
        [HttpGet("{id}")]
        public ActionResult<UserModel> GetUser(int id)
        {
            UserModel user = _context.Users.FirstOrDefault(x => x.Id == id);
            if(user == null)
            {
                return BadRequest("404 - User not found");
            }
            return Ok(user);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //// POST api/<UserController>
        //[HttpPost]
        //public async Task<UserModel> AddUser([FromBody]UserModel userToAdd)
        //{
        //    _context.Users.Add(userToAdd);
        //    return userToAdd;
        //}

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
