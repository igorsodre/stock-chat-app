using StockChatApp.Web.Contracts.Api.Requests;

namespace StockChatApp.Web.Interfaces;

public interface ICommandProcessor
{
    void ProcessCommand(CommandApiRequest commandApiRequest);
}
