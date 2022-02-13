using System.Text.Json;
using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Models;
using StockChatApp.Bot.Models.Commands;
using StockChatApp.Bot.Options;
using StockChatApp.Bot.Producers;

namespace StockChatApp.Bot.Services;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private IStockApiService _stockApiService;
    private ICsvStockParser _csvStockParser;
    private IStockResultFormatter _resultFormatter;
    private readonly StockProducer _stockProducer;

    public EventProcessor(IServiceScopeFactory scopeFactory, HubSettings hubSettings)
    {
        _scopeFactory = scopeFactory;
        _stockProducer = new StockProducer(hubSettings);
    }

    public async Task ProcessEvent(string message)
    {
        using var scope = _scopeFactory.CreateScope();
        _stockApiService = scope.ServiceProvider.GetRequiredService<IStockApiService>();
        _csvStockParser = scope.ServiceProvider.GetRequiredService<ICsvStockParser>();
        _resultFormatter = scope.ServiceProvider.GetRequiredService<IStockResultFormatter>();

        var command = JsonSerializer.Deserialize<BotCommandDto<GetStockDto>>(message)!;
        var apiResult = await _stockApiService.GetStockCsvStringAsync(command.Data.StockCode);
        var parseCsvResult = _csvStockParser.ParseString(apiResult);
        await _stockProducer.PublishStockMessage(_resultFormatter.FormatStock(parseCsvResult));
    }
}
