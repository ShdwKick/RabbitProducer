using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace RabbitProducer;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Базовый путь приложения
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Чтение appsettings.json
            .Build();
        
        var rabbitMqConfig = config.GetSection("RabbitMQ");
        var hostName = rabbitMqConfig["HostName"];
        var userName = rabbitMqConfig["UserName"];
        var password = rabbitMqConfig["Password"];
        
        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var exchangeName = "my_exchange";
        var queueName = "hello";
        var routingKey = "my_routing_key";

        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
        await channel.QueueDeclareAsync(queue: queueName,
            durable: false, exclusive: false,
            autoDelete: false, arguments: null);

        await channel.QueueBindAsync(queueName, exchangeName, routingKey, null);

        var props = new BasicProperties();
        props.ContentType = "text/plain";
        props.DeliveryMode = DeliveryModes.Persistent;

        Console.WriteLine("Enter message to send");

        var sb = new StringBuilder();

        while (channel.IsOpen)
        {
            try
            {
                sb.Append(Console.ReadLine());
                if (sb.Length == 0)
                {
                    sb.Clear();
                    continue;
                }

                await channel.BasicPublishAsync(exchange: exchangeName, 
                    routingKey: routingKey, mandatory: false,
                    basicProperties: props, 
                    body: Encoding.UTF8.GetBytes(sb.ToString()));
                
                Console.WriteLine($"Message: \"{sb.ToString()}\" was sent");
                sb.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}