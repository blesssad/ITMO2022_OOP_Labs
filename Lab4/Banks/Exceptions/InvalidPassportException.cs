namespace Banks.Exceptions;

public class InvalidPassportException : Exception
{
    public InvalidPassportException(string error)
    {
        Error = error;
    }

    public string Error { get; }
}
