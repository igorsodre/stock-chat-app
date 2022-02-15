using StockChatApp.Web.Contracts.Api.Requests;
using StockChatApp.Web.Interfaces;
using StockChatApp.Web.Producers;

namespace StockChatApp.Web.Services;

public class CommandProcessor : ICommandProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Dictionary<string, Type> _producers;

    public CommandProcessor(IServiceScopeFactory scopeFactory)
    {
        _producers = new Dictionary<string, Type> { { "/stock", typeof(StockRequestProducer) } };
        _scopeFactory = scopeFactory;
    }

    public void ProcessCommand(CommandApiRequest commandApiRequest)
    {
        using var scope = _scopeFactory.CreateScope();
        var producerType = _producers[commandApiRequest.Command];
        var commandProcessor = (IProducer)scope.ServiceProvider.GetService(producerType);
        commandProcessor?.ProduceMessage(commandApiRequest.Arguments, commandApiRequest.ConnectionId);
    }
}
