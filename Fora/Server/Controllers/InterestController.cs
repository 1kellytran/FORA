using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public InterestController(AppDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: api/<InterestController>
        [HttpGet]
        public async Task<List<InterestModel>> GetAllInterests()
        {
            //return _context.Interests.ToList();
            List<InterestModel> interests = _context.Interests.Include(i => i.Threads).Select(i => new InterestModel()
            {
                Id = i.Id,
                Name = i.Name,
                UserId = i.UserId,
                Threads = i.Threads.Select(i => new ThreadModel()
                {
                    Id = i.Id,
                    InterestId = i.InterestId,
                    Name = i.Name,
                    UserId = i.UserId


                }).ToList()

            }).ToList();
            return interests;
        }
        [HttpGet]
        [Route("check")]
        public async Task<List<InterestModel>> GetUserInterests([FromQuery] int activeUserid)
        {

            return _context.Interests.Where(i => i.UserInterests.Any(ui => ui.UserId == activeUserid)).Include(i => i.Threads).Select(i => new InterestModel()
            {
                Id = i.Id,
                Name = i.Name,
                UserId = i.UserId,
                Threads = i.Threads.Select(i => new ThreadModel()
                {
                    Id = i.Id,
                    InterestId = i.InterestId,
                    Name = i.Name,
                    UserId = i.UserId


                }).ToList()

            }).ToList();

        }

        [HttpPost]
        [Route("uta")]
        public async Task AddExistingUserInterest([FromBody]UserInterestModel userInterestToAdd)
        {
            var activeUser= _context.Users.FirstOrDefault(x => x.Id == userInterestToAdd.User.Id);
            var currentInterest= _context.Interests.FirstOrDefault(x => x.Id == userInterestToAdd.Interest.Id);

            userInterestToAdd.User = activeUser;
            userInterestToAdd.Interest = currentInterest;
           
             await _context.UserInterests.AddAsync(userInterestToAdd);
             await _context.SaveChangesAsync();

        }

        

        // GET api/<InterestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

         //POST api/<InterestController>
        //[HttpPost]
        //public async Task CreateInterest([FromBody] InterestModel interestToAdd)
        //{
        //    InterestModel interestModel = new();
        //    UserInterestModel userInterestModel = new();

        //    await _context.Interests.AddAsync(interestToAdd);
        //    await _context.SaveChangesAsync();

        //    interestModel = _context.Interests.FirstOrDefault(x => x.Name == interestToAdd.Name);
        //    userInterestModel.InterestId = interestToAdd.Id;
        //    userInterestModel.UserId = (int)interestToAdd.UserId;
        //    var result = await _context.UserInterests.AddAsync(userInterestModel);
        //    await _context.SaveChangesAsync();
        //}

        [HttpPost]
        [Route("create")]
        public async Task CreateInterest([FromBody] InterestModel interestToAdd, [FromQuery]string token)
        {

            var authUser = _signInManager.UserManager.Users.FirstOrDefault(u => u.Token == token);
            var user = _context.Users.FirstOrDefault(u => u.Username == authUser.UserName);

            // Add interest to all interests
            await _context.Interests.AddAsync(new InterestModel()
            {
                Name = interestToAdd.Name,
                User = user,
            });
            await _context.SaveChangesAsync();

            // Add interest to user interests

            var interest = _context.Interests.FirstOrDefault(i => i.Name == interestToAdd.Name);

            var result = await _context.UserInterests.AddAsync(new UserInterestModel()
            {
                User = user,
                Interest = interest
            });
            await _context.SaveChangesAsync();
        }

        // PUT api/<InterestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<InterestController>/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteInterest(int id)
        {
            string message = "";

            InterestModel interestToDelete = new();
            interestToDelete = _context.Interests.FirstOrDefault(x => x.Id == id);
            if (interestToDelete != null)
            {
                _context.Interests.Remove(interestToDelete);
                await _context.SaveChangesAsync();
                return message = "Interest deleted!";
            }
            else return message = "Something went wrong, can not delete user!";
        }

        [HttpDelete]
        [Route("removefromfav")]
        public async Task RemoveInterestFromFav([FromQuery] int interestID, int userID)
        {
            var interestList = _context.UserInterests.Where(x => x.InterestId == interestID).ToList(); 
            var getInterestForUser= interestList.FirstOrDefault(x =>x.UserId == userID);
             _context.UserInterests.Remove(getInterestForUser);
            await _context.SaveChangesAsync();

        }

        [HttpDelete]
        [Route("deletefrominterest")]
        public async Task RemoveInterestFromFav([FromQuery] int interestId )
        {
            var interestList = _context.Interests.FirstOrDefault(x => x.Id == interestId);
           
            _context.Interests.Remove(interestList);
            await _context.SaveChangesAsync();

        }

    }
}