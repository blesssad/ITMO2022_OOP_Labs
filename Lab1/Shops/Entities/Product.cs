using Shops.Exceptions;

namespace Shops.Entities;

public class Product
{
    public Product(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new InvalidNameException("string is null or WhiteSpace");
        }

        ProductName = productName;
    }

    public string ProductName { get; }
}
