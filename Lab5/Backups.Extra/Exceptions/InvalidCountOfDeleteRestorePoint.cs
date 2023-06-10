namespace Backups.Extra.Exceptions;

public class InvalidCountOfDeleteRestorePoint : Exception
{
    public InvalidCountOfDeleteRestorePoint(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
