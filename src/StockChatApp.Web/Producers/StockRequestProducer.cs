using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using StockChatApp.Web.Contracts.Producers;
using StockChatApp.Web.Interfaces;
using StockChatApp.Web.Options;

namespace StockChatApp.Web.Producers;

public class StockRequestProducer : IProducer<CommandDto<StockRequestDto>>
{
    private readonly ILogger<StockRequestProducer> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public StockRequestProducer(RabbitMqSettings queueSettings, ILogger<StockRequestProducer> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory
        {
            HostName = queueSettings.Host,
            Port = queueSettings.Port
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not connet to RabbitMQ server");
            throw;
        }
    }

    public void ProduceMessage(CommandDto<StockRequestDto> content)
    {
        if (_connection.IsOpen)
        {
            var message = JsonSerializer.Serialize(content);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("trigger", "", null, messageBytes);
        }
        else
        {
            _logger.LogError("Could not publish message");
        }
    }

    public void Dispose()
    {
        if (!_channel.IsOpen)
            return;

        _channel.Close();
        _connection.Close();
    }
}
