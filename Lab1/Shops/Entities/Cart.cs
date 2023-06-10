using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;
public class Cart
{
    public Cart()
    {
        ProductsInCart = new List<ProductAndCount>();
    }

    public IReadOnlyList<ProductAndCount> Products
    {
        get
        {
            return ProductsInCart;
        }
    }

    private List<ProductAndCount> ProductsInCart { get; set; }

    public void AddToCart(Product newProduct, int count)
    {
        ArgumentNullException.ThrowIfNull(nameof(newProduct));

        if (count <= 0)
        {
            throw new InvalidCountException("negative or zero value of product");
        }

        var productAndCount = new ProductAndCount(newProduct, count);

        ProductsInCart.Add(productAndCount);
    }
}
