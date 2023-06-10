using Backups.Extra.Services;

namespace Backups.Extra.Loggers;

public class FileLogger : ILogger
{
    private readonly string _fileName;

    public FileLogger(string fileName)
    {
        _fileName = fileName;
    }

    public void Log(string logMessage)
    {
        using var writer = new StreamWriter(_fileName, true);

        writer.WriteLine(logMessage);
    }
}
