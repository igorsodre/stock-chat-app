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
        _connection = new HubConnectionBuilder().WithUrl($"{hubSettings.BaseUrl}/hubs/chat").Build();
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
    
    // private readonly ILogger<StockRequestProducer> _logger;
    // private readonly RabbitMqSettings _queueSettings;
    // private readonly IConnection _connection;
    // private readonly IModel _channel;
    //
    // public StockRequestProducer(RabbitMqSettings queueSettings, ILogger<StockRequestProducer> logger)
    // {
    //     _queueSettings = queueSettings;
    //     _logger = logger;
    //     var factory = new ConnectionFactory
    //     {
    //         HostName = queueSettings.Host,
    //         Port = queueSettings.Port
    //     };
    //
    //     try
    //     {
    //         _connection = factory.CreateConnection();
    //         _channel = _connection.CreateModel();
    //         _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
    //     }
    //     catch (Exception exception)
    //     {
    //         _logger.LogError(exception, "Could not connet to RabbitMQ server");
    //         throw;
    //     }
    // }
    //
    // public void PublishMessage()
    // {
    //     if (_connection.IsOpen)
    //     {
    //         var message = JsonSerializer.Serialize(new { key = "Value" });
    //         var messageBytes = Encoding.UTF8.GetBytes(message);
    //         _channel.BasicPublish("trigger", "", null, messageBytes);
    //     }
    //     else
    //     {
    //         _logger.LogError("Could not publish message");
    //     }
    // }
    //
    // public void Dispose()
    // {
    //     if (_channel.IsOpen)
    //     {
    //         _channel.Close();
    //         _connection.Close();
    //     }
    // }
}
