using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
 
namespace BikeDistributor
{
    public class BikeDiscounts
    {
        public Dictionary<int, DiscountInfo> Discounts { get; } = new Dictionary<int, DiscountInfo>();

        public BikeDiscounts((int quantity, DiscountInfo di)[] arr) => Add(arr);
        public BikeDiscounts(int quantity, DiscountInfo di) => Add(quantity, di);

        public void Add(int quantity, DiscountInfo di)
        {
            Debug.Assert(quantity > 0 || quantity == -1);

            if (quantity <= 0 && quantity != -1)
                throw new ArgumentException("Must be greater than zero or -1 as a default catch all discount. "
                    + "Typically used for complex discounts that can't be handled by simple addition or multiplaction.", nameof(quantity));

            if (Discounts.ContainsKey(quantity))
                throw new ArgumentException(string.Format("The key value [{0}] already exists. Duplicates are not allowed.", quantity), nameof(quantity));

            Discounts.Add(quantity, di);
        }

        public void Add((int quantity, DiscountInfo di)[] arr)
        {
            foreach (var i in arr)
            {
                Add(i.quantity, i.di);
            }
        }

        public DiscountInfo GetDiscount(int quantity)
        {
            Debug.Assert(quantity > 0);

            if (quantity <= 0)
                throw new ArgumentException("Must be greater than zero", nameof(quantity));

            var d = Discounts.Where((arg) => (arg.Key <= quantity));
            if (!d.Any())
            {
                return new DiscountInfo(DiscountInfo.DiscountTypeFlag.None, null, null);
            }
            var k = d.Max((arg) => arg.Key);
            return d.First((arg) => arg.Key == k).Value;
        }
    }
}
