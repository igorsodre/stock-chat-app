using Microsoft.AspNetCore.Mvc;
using StockChatApp.Web.Contracts.Api.Requests;
using StockChatApp.Web.Contracts.Api.Responses;
using StockChatApp.Web.Interfaces;

namespace StockChatApp.Web.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandProcessor _commandProcessor;

        public CommandsController(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        [HttpPost("")]
        public IActionResult PlaceCommand(CommandApiRequest request)
        {
            _commandProcessor.ProcessCommand(request);
            return Ok(Success.Default());
        }
    }
}
