using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using StockChatApp.Bot.Options;

namespace StockChatApp.Bot.Producers;

public class StockProducer
{
    private readonly HubConnection _connection;

    public StockProducer(IOptions<HubSettings> hubSettings)
    {
        _connection = new HubConnectionBuilder().WithUrl(
                $"{hubSettings.Value.BaseUrl}/hubs/chat",
                options => {
                    options.HttpMessageHandlerFactory = handler => {
                        if (handler is HttpClientHandler clientHandler)
                        {
                            clientHandler.ServerCertificateCustomValidationCallback += (_, _, _, _) => true;
                        }

                        return handler;
                    };
                }
            )
            .Build();
    }

    public async Task PublishStockMessage(string message, string connectionId)
    {
        if (IsHubDisconnected())
        {
            await _connection.StartAsync();
        }

        await _connection.InvokeAsync("SendMessageToUser", connectionId, "[BOT] StockABot", message);
    }

    private bool IsHubDisconnected()
    {
        return _connection.State == HubConnectionState.Disconnected;
    }
}
