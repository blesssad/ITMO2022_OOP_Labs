using Shops.Models;

namespace Shops.Entities;
public class Order
{
    public Order(IReadOnlyList<ProductConsignment> orderProducts, Shop shop, Buyer buyer)
    {
        OrderProducts = orderProducts;
        Shop = shop;
        Buyer = buyer;
    }

    public Shop Shop { get; }
    public Buyer Buyer { get; }
    public IReadOnlyList<ProductConsignment> OrderProducts { get; }
}
