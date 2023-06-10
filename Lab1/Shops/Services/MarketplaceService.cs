using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Services
{
    public class MarketplaceService : IMarketplaceService
    {
        private const int StartCost = 0;
        private readonly Dictionary<string, Shop> _shops;
        private int _id;
        public MarketplaceService()
        {
            _shops = new Dictionary<string, Shop>();
            _id = 1;
        }

        public Shop AddShop(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidNameException("string is null or WhiteSpace");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new InvalidNameException("string is null or WhiteSpace");
            }

            var newShop = new Shop(address, name, _id);
            _id++;
            _shops.Add(name, newShop);

            return newShop;
        }

        public void Supply(IReadOnlyList<ProductAndCount> storage, Shop shop)
        {
            ArgumentNullException.ThrowIfNull(nameof(shop));

            foreach (ProductAndCount iter in storage)
            {
                if (!iter.Count.Equals(0))
                {
                    var newTuple = new ProductConsignment(iter.Count, iter.Product, StartCost);
                    shop.AddProduct(newTuple);
                }
            }
        }

        public Order? MakeOrder(Buyer buyer, Shop shop, Product product, int amount)
        {
            ArgumentNullException.ThrowIfNull(nameof(buyer));

            ArgumentNullException.ThrowIfNull(nameof(shop));

            ArgumentNullException.ThrowIfNull(nameof(product));

            if (amount > shop.FindProduct(product) !.Count)
            {
                return null;
            }

            if (buyer.Money < (shop.FindProduct(product) !.Cost * amount))
            {
                return null;
            }

            Order? newOrder = buyer.BuyProduct(shop, product, amount);

            if (newOrder != null)
            {
                shop.SellProduct(product, amount);
            }

            return newOrder;
        }

        public Order? MakeOrderCart(Buyer buyer, Shop shop)
        {
            ArgumentNullException.ThrowIfNull(nameof(buyer));

            ArgumentNullException.ThrowIfNull(nameof(shop));

            ArgumentNullException.ThrowIfNull(nameof(buyer.BuyerCart));

            Order? newOrder = buyer.BuyCart(shop);

            if (newOrder != null)
            {
                shop.SellCart(buyer.BuyerCart);
            }

            return newOrder;
        }

        public Shop? FindShopWithCheapestPrice(Cart cart)
        {
            Shop? minShop = null;
            decimal minValue = int.MaxValue;
            decimal tempValue = 0;

            foreach (Shop shop in _shops.Values)
            {
                foreach (ProductAndCount productTuple in cart.Products)
                {
                    if ((shop.FindProduct(productTuple.Product) is null) || (shop.FindProduct(productTuple.Product) !.Count < productTuple.Count))
                    {
                        tempValue = decimal.MaxValue;
                        break;
                    }
                    else
                    {
                        tempValue += shop.FindProduct(productTuple.Product) !.Cost * productTuple.Count;
                    }
                }

                if (tempValue < minValue)
                {
                    minShop = shop;
                    minValue = tempValue;
                }

                tempValue = 0;
            }

            return minShop;
        }
    }
}