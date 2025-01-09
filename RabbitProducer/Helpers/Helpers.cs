namespace RabbitProducer;

public class Helpers
{
    public static void PrintMessageSentMessage(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\tMessage: \"{msg}\" was sent.");
        Console.ResetColor();
    }

    public static void PrintErrorMessage(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\tAn error occurred: {msg}");
        Console.ResetColor();
    }
}