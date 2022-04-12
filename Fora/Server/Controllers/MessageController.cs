using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<MessageController>
        [HttpGet]
        [Route("allMessages")]
        public async Task<List<MessageModel>> GetAllMessages([FromQuery]int threadID)
        {
            List<MessageModel> messages = new();

            messages = _context.Messages.Where(m => m.ThreadId == threadID).ToList();
            return messages;
        }

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MessageController>
        [HttpPost]
        public async void CreateMessage([FromBody]MessageModel messageToAdd)
        {
            var interest = _context.Threads.FirstOrDefault(i => i.Id == messageToAdd.Thread.Id);
            var user = _context.Users.FirstOrDefault(u => u.Id == messageToAdd.User.Id);

            messageToAdd.User = user;

            await _context.Messages.AddAsync(messageToAdd);
            await _context.SaveChangesAsync();
        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
