using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Shop
{
    public Shop(string shopAddress, string shopName, int id)
    {
        if (string.IsNullOrWhiteSpace(shopAddress))
        {
            throw new InvalidNameException("string is null or WhiteSpace");
        }

        if (string.IsNullOrWhiteSpace(shopName))
        {
            throw new InvalidNameException("string is null or WhiteSpace");
        }

        if (id <= 0)
        {
            throw new InvalidShopIdException("Shop ID negtive or zero");
        }

        ShopAddress = shopAddress;
        ShopName = shopName;
        Id = id;
        ShopMoney = 0;
        Products = new List<ProductConsignment>();
    }

    public string ShopAddress { get; }
    public string ShopName { get; }
    public int Id { get; }
    public decimal ShopMoney { get; private set; }
    public IReadOnlyCollection<ProductConsignment> ProductsToBuyer
    {
        get
        {
            return Products;
        }
    }

    private List<ProductConsignment> Products { get; set; }

    public void ChangeProductCost(Product product, decimal newCost)
    {
        ArgumentNullException.ThrowIfNull(nameof(product));

        if (newCost <= 0)
        {
            throw new InvalidMoneyQuanityException("negative value of money");
        }

        ProductConsignment? item = Products
        .SingleOrDefault(p => p.Product == product);

        if (item != null)
        {
            item.ChangeCost(newCost);
        }
    }

    public ProductConsignment? FindProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(nameof(product));

        ProductConsignment? find = Products
            .SingleOrDefault(p => p.Product == product);

        return find;
    }

    public void AddProduct(ProductConsignment productConsignment)
    {
        ArgumentNullException.ThrowIfNull(nameof(productConsignment));

        Products.Add(productConsignment);
    }

    public void SellCart(Cart cart)
    {
        ArgumentNullException.ThrowIfNull(nameof(cart));

        foreach (ProductAndCount productTuple in cart.Products)
        {
            ArgumentNullException.ThrowIfNull(nameof(productTuple.Product));

            if (FindProduct(productTuple.Product) !.Count < productTuple.Count)
            {
                throw new InvalidCountException("Count in Cart more then Count in Shop");
            }

            FindProduct(productTuple.Product) !.ChangeCount(FindProduct(productTuple.Product) !.Count - productTuple.Count);
            ChangeMoney(ShopMoney + (FindProduct(productTuple.Product) !.Cost * productTuple.Count));
        }
    }

    public void SellProduct(Product product, int amount)
    {
        ArgumentNullException.ThrowIfNull(nameof(product));

        if (amount < 0)
        {
            throw new InvalidCountException("negative amount value");
        }

        FindProduct(product) !.ChangeCount(FindProduct(product) !.Count - amount);
        ChangeMoney(FindProduct(product) !.Cost * amount);
    }

    private void ChangeMoney(decimal newMoney)
    {
        if (newMoney < 0)
        {
            throw new InvalidMoneyQuanityException("negative value of money");
        }

        ShopMoney = newMoney;
    }
}
