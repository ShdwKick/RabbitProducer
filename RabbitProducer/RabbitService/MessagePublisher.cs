using System.Text;
using RabbitMQ.Client;

namespace RabbitProducer.RabbitService;

public class MessagePublisher
{
    private readonly IChannel _channel;

    public MessagePublisher(IChannel channel)
    {
        _channel = channel;
    }

    public async Task InitializeAsync(string exchangeName, string queueName, string routingKey)
    {
        await Task.Run(() =>
        {
            _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
            _channel.QueueDeclareAsync(queueName, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBindAsync(queueName, exchangeName, routingKey);
        });
    }
    
    public async Task PublishMessageAsync(string exchangeName, string routingKey, string message)
    {
        var props = new BasicProperties
        {
            ContentType = "text/plain",
            DeliveryMode = DeliveryModes.Persistent
        };

        var body = Encoding.UTF8.GetBytes(message);

        await Task.Run(() =>
        {
            _channel.BasicPublishAsync(exchangeName, routingKey, mandatory: false, basicProperties: props, body: body);
        });
    }
}