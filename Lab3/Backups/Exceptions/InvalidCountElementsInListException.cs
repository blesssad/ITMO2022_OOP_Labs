namespace Backups.Exceptions;

public class InvalidCountElementsInListException : Exception
{
    public InvalidCountElementsInListException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
