using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using StockChatApp.Bot.Models;
using StockChatApp.Bot.Options;

namespace StockChatApp.Bot.Producers;

public class StockProducer
{
    private readonly HubConnection _connection;

    public StockProducer(HubSettings hubSettings)
    {
        _connection = new HubConnectionBuilder().WithUrl(
                $"{hubSettings.BaseUrl}/hubs/chat",
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

    public async Task PublishStockMessage(string message)
    {
        if (IsHubDisconnected())
        {
            await _connection.StartAsync();
        }

        await _connection.InvokeAsync("SendMessage", "[BOT] StockABot", message);
    }

    private bool IsHubDisconnected()
    {
        return _connection.State == HubConnectionState.Disconnected;
    }
}
