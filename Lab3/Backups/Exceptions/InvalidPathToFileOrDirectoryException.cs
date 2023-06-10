namespace Backups.Exceptions;

public class InvalidPathToFileOrDirectoryException : Exception
{
    public InvalidPathToFileOrDirectoryException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}