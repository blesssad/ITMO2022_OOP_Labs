namespace Backups.Exceptions;

public class InvalidTypeOfBackupObjectException : Exception
{
    public InvalidTypeOfBackupObjectException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
