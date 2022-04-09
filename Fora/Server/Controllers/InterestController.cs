using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET api/<InterestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InterestController>
        [HttpPost]
        public async Task CreateInterest([FromBody]InterestModel interestToAdd)
        {
            await _context.Interests.AddAsync(interestToAdd);
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
            else return message = "Something went wrong, cannot delete user!";
        }
    }
}
