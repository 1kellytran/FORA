using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InterestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<InterestController>
        [HttpGet]
        public async Task<List<InterestModel>> GetAllInterests()
        {
            return _context.Interests.ToList();
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

        [HttpGet]
        [Route("check")]
        public async Task<List<InterestModel>> GetUserInterests([FromQuery] int activeUserid)
        {
     
            return _context.Interests.Where(i => i.UserInterests.Any(ui => ui.UserId == activeUserid)).ToList();

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
        public async Task CreateInterest([FromBody] InterestModel interestToAdd)
        {
            InterestModel interestModel = new();
            UserInterestModel userInterestModel = new();
            UserModel activeUser = new();

            await _context.Interests.AddAsync(interestToAdd);
            await _context.SaveChangesAsync();



            interestModel = _context.Interests.FirstOrDefault(x => x.Name == interestToAdd.Name);
            activeUser = _context.Users.FirstOrDefault(x => x.Id == interestModel.UserId);

            userInterestModel.Interest= interestToAdd;

            userInterestModel.User = activeUser;

            
           

            var result = await _context.UserInterests.AddAsync(userInterestModel);
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