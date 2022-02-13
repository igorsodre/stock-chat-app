using StockChatApp.Bot.Options;

namespace StockChatApp.Bot.Consumers;

public class StockRequestConsumer : BackgroundService
{
    private readonly WorkerSettings _workerSettings;

    public StockRequestConsumer(WorkerSettings workerSettings)
    {
        _workerSettings = workerSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_workerSettings.LoopInterval, stoppingToken);
        }
    }
}
