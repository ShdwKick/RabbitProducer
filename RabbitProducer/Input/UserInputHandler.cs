namespace RabbitProducer.Input;

public class UserInputHandler
{
    public static string ReadMessage()
    {
        Console.Write("\nPlease enter your message: ");
        return Console.ReadLine() ?? string.Empty;
    }
}