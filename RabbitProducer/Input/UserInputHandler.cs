namespace RabbitProducer.Input;

public class UserInputHandler
{
    public static string ReadMessage()
    {
        Console.Write("Please enter your message: ");
        return Console.ReadLine() ?? string.Empty;
    }
}