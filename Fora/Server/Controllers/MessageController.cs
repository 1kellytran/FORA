using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<MessageModel>> GetAllMessages([FromQuery] int threadID)
        {
            List<MessageModel> messages = new();

            messages = _context.Messages.Include(m => m.User).Where(m => m.ThreadId == threadID)
                .Select(m => new MessageModel()
                {
                    Id = m.Id,
                    Message = m.Message,
                    Created = m.Created,
                    UserId = m.UserId,
                    Deleted = m.Deleted,
                    Edited = m.Edited,
                    User = new UserModel()
                    {
                        Id = m.User.Id,
                        Username = m.User.Username,
                    }
                }).ToList();
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
        public async void CreateMessage([FromBody] MessageModel messageToAdd)
        {
            var thread = _context.Threads.FirstOrDefault(i => i.Id == messageToAdd.Thread.Id);
            var user = _context.Users.FirstOrDefault(u => u.Id == messageToAdd.User.Id);

            messageToAdd.Thread = thread;
            messageToAdd.User = user;
            messageToAdd.Created = DateTime.Now;
            messageToAdd.Deleted = false;
            messageToAdd.Edited = false;

            await _context.Messages.AddAsync(messageToAdd);
            _context.SaveChanges();
        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        [Route("editMessage")]
        public async Task EditMessage(int messageId, [FromBody]MessageModel messageToEdit)
        {
            MessageModel message = new();

            message = _context.Messages.FirstOrDefault(m => m.Id == messageId);

            message.Id = messageToEdit.Id;
            message.UserId = messageToEdit.UserId;
            message.Message = messageToEdit.Message;
            message.ThreadId = messageToEdit.ThreadId;
            message.Edited = true;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        [Route("deleteMessage")]
        public async Task<string> Delete(int messageID)
        {
            string message = "";

            //MessageModel messageToDelete = new();
            //messageToDelete = _context.Messages.FirstOrDefault(m => m.Id == messageID);

            //if (messageToDelete != null)
            //{
            //    _context.Messages.Remove(messageToDelete);
            //    await _context.SaveChangesAsync();
            //    return message = "Message deleted";
            //}
            //else
            //{
            //    return message = "Something went wrong, unable to delete message";
            //}

            MessageModel messageToDelete = new();
            messageToDelete = _context.Messages.FirstOrDefault(m => m.Id == messageID);

            if(messageToDelete != null)
            {
                messageToDelete.Deleted = true;
                messageToDelete.Message = "Message has been deleted";

                await _context.SaveChangesAsync();
                return message = "Message has been deleted";
            }
            else
            {
                return message = "Something went wrong, unable to delete message";
            }
        }
    }
}
