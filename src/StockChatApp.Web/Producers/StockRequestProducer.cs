using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using StockChatApp.Web.Contracts.Producers;
using StockChatApp.Web.Interfaces;

namespace StockChatApp.Web.Producers;

public class StockRequestProducer : IProducer
{
    private readonly ILogger<StockRequestProducer> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string Exchange = "trigger";
    private const string RoutingKey = "";

    public StockRequestProducer(IConnectionFactory factory, ILogger<StockRequestProducer> logger)
    {
        _logger = logger;

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(Exchange, ExchangeType.Fanout);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not connet to RabbitMQ server");
            throw;
        }
    }

    public void ProduceMessage(string arguments, string connectionId)
    {
        if (_connection.IsOpen)
        {
            var message = JsonSerializer.Serialize(
                new CommandDto<StockRequestDto>()
                {
                    Command = "/stock",
                    Data = new StockRequestDto
                    {
                        Channel = "General Chat",
                        StockCode = arguments,
                        ConnectionId = connectionId
                    }
                }
            );
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(Exchange, RoutingKey, null, messageBytes);
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
