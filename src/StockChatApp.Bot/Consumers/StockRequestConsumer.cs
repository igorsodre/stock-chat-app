using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StockChatApp.Bot.Interfaces;

namespace StockChatApp.Bot.Consumers;

public class StockRequestConsumer : BackgroundService
{
    private readonly IEventHandler _eventHandler;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private const string Exchange = "trigger";
    private const string RoutingKey = "";

    public StockRequestConsumer(IConnectionFactory factory, IEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(Exchange, ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(_queueName, Exchange, RoutingKey);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, eventArgs) => {
            var stringMessage = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            await _eventHandler.ProcessEvent(stringMessage);
        };

        _channel.BasicConsume(_queueName, true, consumer);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        if (_connection.IsOpen)
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        base.Dispose();
    }
}
