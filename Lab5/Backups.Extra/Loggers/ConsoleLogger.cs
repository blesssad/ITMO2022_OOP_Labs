using Backups.Extra.Services;

namespace Backups.Extra.Loggers;

public class ConsoleLogger : ILogger
{
    public void Log(string logMessage)
    {
        Console.WriteLine(logMessage);
    }
}
