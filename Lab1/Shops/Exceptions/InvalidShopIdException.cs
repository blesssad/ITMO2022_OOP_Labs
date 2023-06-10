namespace Shops.Exceptions;
public class InvalidShopIdException : Exception
{
    public InvalidShopIdException(string error)
    {
        Error = error;
    }

    public string Error { get; set; }
}
