using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Models;

public class ProductConsignment
{
    public ProductConsignment(int count, Product product, decimal cost)
    {
        if (count <= 0)
        {
            throw new InvalidCountException("negative value of product");
        }

        if (cost < 0)
        {
            throw new InvalidMoneyQuanityException("negative or zero cost of Product");
        }

        ArgumentNullException.ThrowIfNull(nameof(product));

        Cost = cost;
        Count = count;
        Product = product;
    }

    public int Count { get; private set; }
    public Product Product { get; }

    public decimal Cost { get; private set; }

    public void ChangeCost(decimal newCost)
    {
        if (newCost < 0)
        {
            throw new InvalidMoneyQuanityException("negative or zero cost of Product");
        }

        Cost = newCost;
    }

    public void ChangeCount(int newCount)
    {
        if (newCount < 0)
        {
            throw new InvalidCountException("negative or zero count of Product");
        }

        Count = newCount;
    }
}
