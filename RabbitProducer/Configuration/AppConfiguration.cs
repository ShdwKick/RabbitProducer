using Microsoft.Extensions.Configuration;

namespace RabbitProducer.Configuration;

public class AppConfiguration
{
    public static RabbitMQConfig LoadRabbitMQConfig()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var rabbitMqConfig = config.GetSection("RabbitMQ").Get<RabbitMQConfig>();
        
        if (rabbitMqConfig == null)
        {
            throw new InvalidOperationException("RabbitMQ configuration is missing or invalid.");
        }
        return rabbitMqConfig;
    }
}