namespace Shops.Exceptions;

public class InvalidNameException : Exception
{
    public InvalidNameException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
