using RabbitMQ.Client;
using RabbitProducer.Configuration;

namespace RabbitProducer.RabbitService;

public class RabbitMQConnectionFactory
{
    private readonly RabbitMQConfig _config;

    public RabbitMQConnectionFactory(RabbitMQConfig config)
    {
        _config = config;
    }

    public async Task<IConnection> CreateConnectionAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password
        };
        return await factory.CreateConnectionAsync();
    }
}