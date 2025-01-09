using Serilog;
using RabbitProducer.Configuration;
using RabbitProducer.Input;
using RabbitProducer.Logger;
using RabbitProducer.RabbitService;

namespace RabbitProducer;

class Program
{
    static async Task Main(string[] args)
    {
        var logger = SerilogLogger.GetInstacne();
        
        var config = AppConfiguration.LoadRabbitMQConfig();
        
        var connectionFactory = new RabbitMQConnectionFactory(config);
        using var connection = await connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        
        var publisher = new MessagePublisher(channel);
        const string exchangeName = "my_exchange";
        const string queueName = "hello";
        const string routingKey = "my_routing_key";
        
        await publisher.InitializeAsync(exchangeName, queueName, routingKey);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\t\t\tWelcome to the RabbitMQ Producer");
        Console.ResetColor();
        
        while (channel.IsOpen)
        {
            try
            {
                var message = UserInputHandler.ReadMessage();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    await publisher.PublishMessageAsync(exchangeName, routingKey, message);
                    
                    logger.LogInformation($"Message: \"{message}\" was sent.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred: {ex.Message}");
            }
        }
        
        Log.CloseAndFlush();
    }
}