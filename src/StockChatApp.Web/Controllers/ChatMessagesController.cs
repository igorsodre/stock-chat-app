using Microsoft.AspNetCore.Mvc;
using StockChatApp.Web.Contracts.Api.Responses;
using StockChatApp.Web.Contracts.Hubs.Chat;
using StockChatApp.Web.Interfaces;

namespace StockChatApp.Web.Controllers
{
    [Route("api/chat-messages")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessageRepository _messageRepository;

        public ChatMessagesController(IChatMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpGet("")]
        public async Task<ActionResult<Success<IEnumerable<ChatMessage>>>> GetMessages()
        {
            var result = await _messageRepository.GetMessages();
            return Ok(new Success<IEnumerable<ChatMessage>>(result));
        }
    }
}
