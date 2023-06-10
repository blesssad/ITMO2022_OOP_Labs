namespace Banks.Exceptions;

public class SuspicticiousAccountHasLimitException : Exception
{
    public SuspicticiousAccountHasLimitException(string error)
    {
        Error = error;
    }

    public string Error { get; }
}
