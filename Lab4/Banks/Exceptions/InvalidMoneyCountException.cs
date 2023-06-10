namespace Banks.Exceptions;

public class InvalidMoneyCountException : Exception
{
    public InvalidMoneyCountException(string error)
    {
        Error = error;
    }

    public string Error { get; }
}
