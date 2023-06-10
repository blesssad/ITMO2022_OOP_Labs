using Shops.Entities;
using Shops.Models;

namespace Shops.Services;

public interface IMarketplaceService
{
    Shop AddShop(string name, string address);

    void Supply(IReadOnlyList<ProductAndCount> storage, Shop shop);

    Order? MakeOrder(Buyer buyer, Shop shop, Product product, int amount);
    Order? MakeOrderCart(Buyer buyer, Shop shop);

    Shop? FindShopWithCheapestPrice(Cart cart);
}