using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Models;

public class ProductAndCount
{
    public ProductAndCount(Product product, int count)
    {
        ArgumentNullException.ThrowIfNull(nameof(product));

        if (count < 0)
        {
            throw new InvalidCountException("negative count value");
        }

        Product = product;
        Count = count;
    }

    public Product Product { get; }
    public int Count { get; private set; }

    public void ChangeCount(int count)
    {
        if (count < 0)
        {
            throw new InvalidCountException("negative count value");
        }

        Count = count;
    }
}
