namespace Backups.Extra.Exceptions;

public class RestorePointWasDeletedException : Exception
{
    public RestorePointWasDeletedException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
