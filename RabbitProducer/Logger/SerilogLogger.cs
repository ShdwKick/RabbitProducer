using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace RabbitProducer.Logger;

public class SerilogLogger
{
    private static SerilogLogger? _instance;

    private readonly AnsiConsoleTheme _customTheme = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
    {
        [ConsoleThemeStyle.Text] = "\u001b[37m", // Белый
        [ConsoleThemeStyle.SecondaryText] = "\u001b[90m", // Серый
        [ConsoleThemeStyle.TertiaryText] = "\u001b[30m", // Темно-серый
        [ConsoleThemeStyle.Invalid] = "\u001b[33m", // Желтый
        [ConsoleThemeStyle.Null] = "\u001b[36m", // Голубой
        [ConsoleThemeStyle.Name] = "\u001b[32m", // Зеленый
        [ConsoleThemeStyle.String] = "\u001b[35m", // Фиолетовый
        [ConsoleThemeStyle.Number] = "\u001b[31m", // Красный
        [ConsoleThemeStyle.Boolean] = "\u001b[31m", // Красный
        [ConsoleThemeStyle.Scalar] = "\u001b[37m", // Белый
        [ConsoleThemeStyle.LevelVerbose] = "\u001b[37m", // Белый
        [ConsoleThemeStyle.LevelDebug] = "\u001b[37m", // Белый
        [ConsoleThemeStyle.LevelInformation] = "\u001b[32m", // Зеленый
        [ConsoleThemeStyle.LevelWarning] = "\u001b[33m", // Желтый
        [ConsoleThemeStyle.LevelError] = "\u001b[31m", // Красный
        [ConsoleThemeStyle.LevelFatal] = "\u001b[31;1m" // Ярко-красный
    });
    
    public static SerilogLogger GetInstacne() => _instance ??= new SerilogLogger();

    private SerilogLogger()
    {
        InitializeLogger();
    }

    public void InitializeLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                theme: _customTheme)
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public void LogInformation(string message)
    {
        Log.Information(message);
    }

    public void LogDebug(string message)
    {
        Log.Debug(message);
    }

    public void LogWarning(string message)
    {
        Log.Warning(message);
    }

    public void LogError(string message)
    {
        Log.Error(message);
    }
    public void LogError(Exception ex, string message)
    {
        Log.Error(ex, message);
    }

}