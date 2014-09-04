namespace ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPriceCalculator
    {
        int CalculatePrice();
    }

    public class Offer
    {
        public readonly string Item;
        public readonly int NumberRequiredForDeal;
        public readonly int Saving;

        public Offer(string item, int numberRequiredForDeal, int saving)
        {
            Item = item;
            NumberRequiredForDeal = numberRequiredForDeal;
            Saving = saving;
        }
    }

    public class PriceCalculator : IPriceCalculator
    {
        private readonly ICart cart;
        private readonly Dictionary<string, int> pricesDictionary = new Dictionary<string, int> { { "A99", 50 }, { "B15", 30 }, { "C40", 60 }, { "T34", 99 } };

        private readonly List<Offer> specialOffers = new List<Offer>
                                                     {
                                                         new Offer("A99", 3, 20),
                                                         new Offer("B15", 2, 15)
                                                     };

        public PriceCalculator(ICart cart)
        {
            this.cart = cart;
        }

        public int CalculatePrice()
        {
            var totalsDictionary = CreateTotalsDictionary();

            return CostWithoutSpecialOffers(totalsDictionary) - Savings(totalsDictionary);
        }

        private Dictionary<String, int> CreateTotalsDictionary()
        {
            var items = cart.Items;
            var runningTotalsDictionary = new Dictionary<string, int>();

            foreach (var item in items)
            {
                if (runningTotalsDictionary.ContainsKey(item))
                {
                    runningTotalsDictionary[item]++;
                }
                else
                {
                    runningTotalsDictionary.Add(item, 1);
                }
            }
            return runningTotalsDictionary;
        }

        private int CostWithoutSpecialOffers(Dictionary<string, int> totalsDictionary)
        {
            return totalsDictionary.Sum(product => pricesDictionary[product.Key] * product.Value);
        }

        private int Savings(Dictionary<string, int> totalsDictionary)
        {
            return specialOffers.Where(offer => totalsDictionary.ContainsKey(offer.Item)).Sum(offer => (totalsDictionary[offer.Item] / offer.NumberRequiredForDeal) * offer.Saving);
        }
    }
}