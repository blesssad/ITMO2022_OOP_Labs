namespace Shops.Exceptions;
public class InvalidCountException : Exception
{
    public InvalidCountException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
