namespace Backups.Exceptions;

public class FolderAlreadyExistException : Exception
{
    public FolderAlreadyExistException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
