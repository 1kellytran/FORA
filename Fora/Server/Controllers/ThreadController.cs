using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet]
        [Route("allThreads")]
        public async Task<List<ThreadModel>> GetAllThreads([FromQuery] int interestID)
        {
            List<ThreadModel> threads = new();

            //threads = _context.Threads.Include(t => t.Messages).Where(t => t.InterestId == interestID).ToList();

            threads = _context.Threads.Include(t => t.Messages).Where(t => t.InterestId == interestID).Select(t => new ThreadModel()
            {
                Id = t.Id,
                Name = t.Name,
                UserId = t.UserId,
                User = t.User,
                Messages = t.Messages.Select(m => new MessageModel()
                {
                    Id = m.Id,
                    Message = m.Message,
                    User = m.User
                }).ToList()
            }).ToList();

            return threads;
        }

        // GET api/<ThreadController>/5
        [HttpGet]
        [Route("getById")]
        public async Task<ThreadModel> GetThreadByID(int threadId)
        {
            ThreadModel thread = new();
            thread = _context.Threads.FirstOrDefault(t => t.Id == threadId);
            return thread;
        }

        // POST api/<ThreadController>
        [HttpPost]
        public async Task CreateThread([FromBody]ThreadModel threadToAdd)
        {
            var interest = _context.Interests.FirstOrDefault(i => i.Id == threadToAdd.Interest.Id);
            var user = _context.Users.FirstOrDefault(u => u.Id == threadToAdd.User.Id);

            threadToAdd.Interest = interest;
            threadToAdd.User = user;


            await _context.Threads.AddAsync(threadToAdd);
            await _context.SaveChangesAsync();
        }

        // GET api/<ThreadController>/5
        [HttpGet]
        [Route("getUserThreads")]
        public async Task<List<ThreadModel>> GetActiveUserThreads(int userId)
        {
            List<ThreadModel> threads = new();
            threads = _context.Threads.ToList();
            return threads;
        }

        // DELETE api/<ThreadController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            ThreadModel threadToDelete = new();
            threadToDelete = _context.Threads.FirstOrDefault(t => t.Id == id);
            _context.Threads.Remove(threadToDelete);
            _context.SaveChanges();

        }
    }
}
