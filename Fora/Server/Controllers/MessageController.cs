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

            messages = _context.Messages.Where(m => m.ThreadId == threadID).ToList();
           
            return messages;
        }

     

        // POST api/<MessageController>
        [HttpPost]
        public async void CreateMessage([FromBody] MessageModel messageToAdd)
        {
            var thread = _context.Threads.FirstOrDefault(i => i.Id == messageToAdd.Thread.Id);
            var user = _context.Users.FirstOrDefault(u => u.Id == messageToAdd.User.Id);

            messageToAdd.Thread = thread;
            messageToAdd.User = user;
            messageToAdd.Username = user.Username;
            messageToAdd.Created = DateTime.Now;
            messageToAdd.Deleted = false;
            messageToAdd.Edited = false;

            await _context.Messages.AddAsync(messageToAdd);
            _context.SaveChanges();
        }

        // PUT api/<MessageController>/5
        [HttpPut]
        [Route("editMessage/{messageId}")]
        public async Task EditMessage([FromRoute]int messageId, [FromBody]MessageModel messageToEdit)
        {
            MessageModel message = new();

            message = _context.Messages.FirstOrDefault(m => m.Id == messageId);

            message.Message = messageToEdit.Message;
            message.Edited = true;

            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        [Route("deleteMessage")]
        public async Task<string> Delete(int messageID)
        {
            string message = "";

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
