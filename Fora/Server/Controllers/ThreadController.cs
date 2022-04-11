using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThreadController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<ThreadController>
        //[HttpGet]
        //public async Task<List<ThreadModel>> GetAllThreads(int id)
        //{
        //    InterestModel interest = new();
        //    ThreadModel threads = new();

        //    threads = _context.Threads.Where(t => t.Id == id).ToList();
            
        //    interest = _context.Interests.FirstOrDefault(i => i.Id == id);
        //    return _context.Threads.ToList();
        //}

        // GET api/<ThreadController>/5
        //[HttpGet("{id}")]
        //public async Task<List<ThreadModel>> GetThreadByID(int id)
        //{
        //    return _context.Threads.FirstOrDefault(t => t.InterestId == id).ToList();            
        //}

        // POST api/<ThreadController>
        [HttpPost]
        public async Task CreateThread([FromBody]ThreadModel threadToAdd)
        {
            // Get InterestID 

            // Set InterestID for ThreadModel while creating Thread
            InterestModel interest = new();
            ThreadModel thread = new();

            interest = _context.Interests.FirstOrDefault(i => i.Id == threadToAdd.InterestId);

        }

        // PUT api/<ThreadController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ThreadController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
