using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Buyer
{
    public Buyer(string nickName, decimal money)
    {
        if (string.IsNullOrWhiteSpace(nickName))
        {
            throw new InvalidNameException("string is null or WhiteSpace");
        }

        if (money < 0)
        {
            throw new InvalidMoneyQuanityException("negative value of money");
        }

        NickName = nickName;
        Money = money;
        BuyerCart = new Cart();
    }

    public string NickName { get; }

    public decimal Money { get; private set; }
    public Cart BuyerCart { get; private set; }

    public void AddToCart(Product product, int amount)
    {
        ArgumentNullException.ThrowIfNull(nameof(product));

        if (amount <= 0)
        {
            throw new InvalidCountException("negative value of product");
        }

        BuyerCart.AddToCart(product, amount);
    }

    public Order? BuyCart(Shop shop)
    {
        ArgumentNullException.ThrowIfNull(nameof(shop));

        ArgumentNullException.ThrowIfNull(nameof(BuyerCart));

        var orderProducts = new List<ProductConsignment>();

        foreach (ProductAndCount productTuple in BuyerCart.Products)
        {
            if (Money < shop.FindProduct(productTuple.Product) !.Cost)
            {
                return null;
            }

            ProductConsignment orderProductTuple = shop.FindProduct(productTuple.Product) !;

            orderProductTuple.ChangeCount(productTuple.Count);
            orderProducts.Add(orderProductTuple);

            ChangeMoney(Money - (shop.FindProduct(productTuple.Product) !.Cost * productTuple.Count));
        }

        var newOrder = new Order(orderProducts, shop, this);

        return newOrder;
    }

    public Order? BuyProduct(Shop shop, Product product, int amount)
    {
        ArgumentNullException.ThrowIfNull(nameof(shop));

        ArgumentNullException.ThrowIfNull(nameof(product));

        if (Money < (shop.FindProduct(product) !.Cost * amount))
        {
            return null;
        }

        var orderProducts = new List<ProductConsignment>();
        ProductConsignment orderProductTuple = shop.FindProduct(product) !;

        orderProductTuple.ChangeCount(amount);
        orderProducts.Add(orderProductTuple);

        ChangeMoney(Money - (shop.FindProduct(product) !.Cost * amount));

        var newOrder = new Order(orderProducts, shop, this);

        return newOrder;
    }

    private void ChangeMoney(decimal newMoney)
    {
        if (newMoney < 0)
        {
            throw new InvalidMoneyQuanityException("negative value of money");
        }

        Money = newMoney;
    }
}
