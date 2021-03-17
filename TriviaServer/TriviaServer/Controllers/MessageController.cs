using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;

namespace TriviaServer.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepo;
        public MessageController(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }

        [HttpPost("api/message")]
        public async Task<IActionResult> createMessage(MessageModel message)
        {
            int messageId;
            try
            {
                messageId = await _messageRepo.createMessage(message.FromId, message.ToId, message.Body);
            }
            catch (InvalidOperationException ioe)
            {
                return StatusCode(400);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(messageId);
        }

        [HttpGet("api/messages/player/{fromId}/friend/{toId}/amount/{amount}")]
        public async Task<IActionResult> getNMessages(int fromId, int toId, int amount)
        {
            IEnumerable<MessageModel> messages;
            try
            {
                messages = await _messageRepo.getLastNMessages(fromId, toId, amount);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(messages);
        }

        [HttpGet("api/messages/player/{fromId}/friend/{toId}/start/{start}/end/{end}")]
        public async Task<IActionResult> getMessageInDateRange(int fromId, int toId, DateTimeOffset start, DateTimeOffset end)
        {
            List<MessageModel> messages;
            try
            {
                messages = await _messageRepo.getMessagesBetweenDate(fromId, toId, start, end);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            return Ok(messages);
        }
    }
}
