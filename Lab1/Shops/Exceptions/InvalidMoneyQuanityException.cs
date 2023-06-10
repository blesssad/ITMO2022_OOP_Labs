namespace Shops.Exceptions;
public class InvalidMoneyQuanityException : Exception
{
    public InvalidMoneyQuanityException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
