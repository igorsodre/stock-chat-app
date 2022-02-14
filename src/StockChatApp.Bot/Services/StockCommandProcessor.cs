using System.Text.Json;
using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Models;
using StockChatApp.Bot.Models.Commands;
using StockChatApp.Bot.Producers;

namespace StockChatApp.Bot.Services;

public class StockCommandProcessor : ICommandProcessor
{
    private readonly IStockApiService _stockApiService;
    private readonly ICsvStockParser _csvStockParser;
    private readonly IStockResultFormatter _resultFormatter;
    private readonly StockProducer _stockProducer;

    public StockCommandProcessor(
        IStockApiService stockApiService,
        ICsvStockParser csvStockParser,
        IStockResultFormatter resultFormatter,
        StockProducer stockProducer
    )
    {
        _stockApiService = stockApiService;
        _csvStockParser = csvStockParser;
        _resultFormatter = resultFormatter;
        _stockProducer = stockProducer;
    }

    public async Task ProcessCommand(string message)
    {
        var command = JsonSerializer.Deserialize<BotCommandDto<GetStockDto>>(message)!;
        var apiResult = await _stockApiService.GetStockCsvStringAsync(command.Data.StockCode);
        var parseCsvResult = _csvStockParser.ParseString(apiResult);
        await _stockProducer.PublishStockMessage(
            _resultFormatter.FormatStock(parseCsvResult),
            command.Data.ConnectionId
        );
    }
}
