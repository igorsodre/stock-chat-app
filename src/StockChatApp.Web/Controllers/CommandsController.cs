using Microsoft.AspNetCore.Mvc;
using StockChatApp.Web.Contracts.Api.Requests;
using StockChatApp.Web.Contracts.Api.Responses;
using StockChatApp.Web.Contracts.Producers;
using StockChatApp.Web.Interfaces;

namespace StockChatApp.Web.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly IProducer<CommandDto<StockRequestDto>> _producer;

        public CommandsController(IProducer<CommandDto<StockRequestDto>> producer)
        {
            _producer = producer;
        }

        [HttpPost("stock")]
        public IActionResult PlaceCommand(CommandApiRequest request)
        {
            _producer.ProduceMessage(
                new CommandDto<StockRequestDto>()
                {
                    Command = request.Command,
                    Data = new StockRequestDto
                    {
                        Channel = "General Chat",
                        StockCode = request.Arguments,
                        ConnectionId = request.ConnectionId
                    }
                }
            );
            return Ok(Success.Default());
        }
    }
}
