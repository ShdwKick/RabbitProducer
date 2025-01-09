using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitProducer.Configuration;
using RabbitProducer.Input;
using RabbitProducer.RabbitService;

namespace RabbitProducer;

class Program
{
    static async Task Main(string[] args)
    {
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
        Console.WriteLine("\t\t\tWelcome to the RabbitMQ Producer\n");
        Console.ResetColor();
        
        while (channel.IsOpen)
        {
            try
            {
                var message = UserInputHandler.ReadMessage();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    await publisher.PublishMessageAsync(exchangeName, routingKey, message);
                    
                    Helpers.PrintMessageSentMessage(message);
                }
            }
            catch (Exception ex)
            {
                Helpers.PrintErrorMessage(ex.Message);
            }
        }
    }
}