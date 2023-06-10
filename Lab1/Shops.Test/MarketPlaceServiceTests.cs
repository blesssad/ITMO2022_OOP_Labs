using Shops.Entities;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class MarketPlaceServiceTests
{
    private readonly MarketplaceService service;

    public MarketPlaceServiceTests()
    {
        service = new MarketplaceService();
    }

    [Fact]
    public void SupplyTest()
    {
        Shop shop = service.AddShop("VkusVill", "Moskovsky Prospekt 143");
        var bread = new Product("bread");
        var mango = new Product("mango");
        var storage = new List<ProductAndCount>();
        var mango17 = new ProductAndCount(mango, 17);
        var bread10 = new ProductAndCount(bread, 10);

        storage.Add(bread10);
        storage.Add(mango17);

        service.Supply(storage, shop);

        storage.Remove(bread10);
        storage.Remove(mango17);

        Assert.Equal(mango, shop.FindProduct(mango) !.Product);
    }

    [Fact]
    public void ChangeCost()
    {
        const int newCost = 300;
        Shop shop = service.AddShop("VkusVill", "Moskovsky Prospekt 143");
        var mango = new Product("mango");
        var storage = new List<ProductAndCount>();
        var mango17 = new ProductAndCount(mango, 17);

        storage.Add(mango17);
        service.Supply(storage, shop);
        storage.Remove(mango17);

        shop.ChangeProductCost(mango, newCost);

        Assert.Equal(newCost, shop.FindProduct(mango) !.Cost);
    }

    [Fact]
    public void MakeOrderFirst()
    {
        const int newCost = 250;
        Shop shop = service.AddShop("VkusVill", "Moskovsky Prospekt 143");
        var buyer = new Buyer("Vadim", 1000);
        var mango = new Product("mango");
        var storage = new List<ProductAndCount>();
        var mango57 = new ProductAndCount(mango, 57);

        storage.Add(mango57);
        service.Supply(storage, shop);
        storage.Remove(mango57);

        shop.ChangeProductCost(mango, newCost);

        Order? newOrder = service.MakeOrder(buyer, shop, mango, 3);

        Assert.Equal(newOrder!.Buyer.Money, 1000 - (newCost * 3));
        Assert.Equal(newOrder.Shop.ShopMoney, newCost * 3);
    }

    [Fact]
    public void CheapestShop()
    {
        var buyer = new Buyer("Vadim", 1000);

        Shop shop1 = service.AddShop("first", "Kronverskiy 3");
        Shop shop2 = service.AddShop("second", "Kronverskiy 4");
        Shop shop3 = service.AddShop("third", "Kronverskiy 5");
        Shop shop4 = service.AddShop("forth", "Kronverskiy 6");

        var storage1 = new List<ProductAndCount>();
        var storage2 = new List<ProductAndCount>();
        var storage3 = new List<ProductAndCount>();
        var storage4 = new List<ProductAndCount>();

        var mango = new Product("mango");
        var beer = new Product("beer");

        var mango1 = new ProductAndCount(mango, 1);
        var beer2 = new ProductAndCount(beer, 2);
        var beer5 = new ProductAndCount(beer, 5);
        var mango5 = new ProductAndCount(mango, 5);
        var mango4 = new ProductAndCount(mango, 4);
        var mango3 = new ProductAndCount(mango, 3);

        buyer.AddToCart(mango, 3);
        buyer.AddToCart(beer, 1);

        storage1.Add(mango1);
        storage1.Add(beer2);
        storage2.Add(mango5);
        storage2.Add(beer5);
        storage3.Add(mango4);
        storage3.Add(beer2);
        storage4.Add(mango3);

        service.Supply(storage1, shop1);
        service.Supply(storage2, shop2);
        service.Supply(storage3, shop3);
        service.Supply(storage4, shop4);

        shop1.ChangeProductCost(mango, 40);
        shop1.ChangeProductCost(beer, 20);
        shop2.ChangeProductCost(mango, 70);
        shop2.ChangeProductCost(beer, 30);
        shop3.ChangeProductCost(mango, 60);
        shop3.ChangeProductCost(beer, 20);
        shop4.ChangeProductCost(mango, 10);

        Assert.Equal(service.FindShopWithCheapestPrice(buyer.BuyerCart), shop3);
    }

    [Fact]
    public void MakeOrderSecondCart()
    {
        const int MangoCost = 150;
        const int BeerCost = 299;

        var buyer = new Buyer("Vadim", 1000);
        var mango = new Product("mango");
        var beer = new Product("beer");
        Shop shop = service.AddShop("VkusVill", "Moskovsky Prospekt 143");
        var storage = new List<ProductAndCount>();
        var mango11 = new ProductAndCount(mango, 11);
        var beer20 = new ProductAndCount(beer, 20);

        storage.Add(mango11);
        storage.Add(beer20);
        service.Supply(storage, shop);
        storage.Remove(beer20);
        storage.Remove(mango11);

        shop.ChangeProductCost(mango, MangoCost);
        shop.ChangeProductCost(beer, BeerCost);

        buyer.AddToCart(mango, 3);
        buyer.AddToCart(beer, 1);

        Order? newOrder = service.MakeOrderCart(buyer, shop);

        Assert.Equal(newOrder!.Buyer.Money, 1000 - (MangoCost * 3) - BeerCost);
        Assert.Equal(newOrder.Shop.ShopMoney, (MangoCost * 3) + BeerCost);
    }
}
