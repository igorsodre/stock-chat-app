using System.Text.Json;
using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Models;
using StockChatApp.Bot.Models.Commands;
using StockChatApp.Bot.Options;
using StockChatApp.Bot.Producers;

namespace StockChatApp.Bot.Services;

public class EventHandler : IEventHandler
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Dictionary<string, Type> _processors;

    public EventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _processors = new Dictionary<string, Type> { { "/stock", typeof(StockCommandProcessor) } };
    }

    public Task ProcessEvent(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var command = JsonSerializer.Deserialize<CommandDto>(message);
        if (command is null)
        {
            return Task.CompletedTask;
        }

        var processorType = _processors[command.Command];
        var commandProcessor = (ICommandProcessor)scope.ServiceProvider.GetService(processorType);
        return commandProcessor is null ? Task.CompletedTask : commandProcessor.ProcessCommand(message);
    }
}
