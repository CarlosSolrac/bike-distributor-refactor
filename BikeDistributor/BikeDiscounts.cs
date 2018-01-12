using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BikeDistributor
{
    public class BikeDiscounts
    {
        public BikeDiscounts((int quantity, decimal discount)[] arr)
        {
            Add(arr);
        }

        protected Dictionary<int, decimal> Discounts = new Dictionary<int, decimal>();

        public void Add(int quantity, decimal discount)
        {
            Debug.Assert(quantity > 0);
            Debug.Assert(0 < discount && discount < 1);

            Discounts.Add(quantity, discount);
        }

        public void Add((int quantity, decimal discount)[] arr)
        {
            foreach (var i in arr)
            {
                Add(i.quantity, i.discount);
            }
        }

        public decimal Discount(int quantity)
        {
            Debug.Assert(quantity > 0);

            quantity = 100;
            var d = Discounts.Where((arg) => (arg.Key <= quantity));
            if (!d.Any())
            {
                return 0;
            }
            var k = d.Max((arg) => arg.Key);
            return d.First((arg) => arg.Key == k).Value;
        }
    }
}
