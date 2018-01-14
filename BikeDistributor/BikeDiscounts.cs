using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
 
namespace BikeDistributor
{
    // Design: Separate the Bike properties and discount information
    public class BikeDiscounts
    {
        /// <summary>
        /// Dictionary of discounts.
        /// The key is the quantity level. Duplicate keys are not allowed.
        /// The value is the discount object.
        /// </summary>
        public Dictionary<int, DiscountInfo> Discounts { get; } = new Dictionary<int, DiscountInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BikeDistributor.BikeDiscounts"/> class.
        /// Adds an array of discount objects to the dictionary.
        /// </summary>
        /// <param name="arr">An array of discount objects</param>
        /// 
        /// Design tradeoffs: IEnumerable vs Inline Tupe declaration
        /// Using an IEnumerable is better design practice. But I wanted to use
        /// an inline tuple declaration to play with the new C# features.
        public BikeDiscounts((int quantity, DiscountInfo di)[] arr) => Add(arr);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BikeDistributor.BikeDiscounts"/> class.
        /// Adds a single discount object to the dictionary.
        /// </summary>
        /// <param name="quantity">Quantity.</param>
        /// <param name="di">Discount object</param>
        public BikeDiscounts(int quantity, DiscountInfo di) => Add(quantity, di);


        /// <summary>
        /// Add a single discount to the dictionary.
        /// </summary>
        /// <param name="quantity">Quantity level for discount to occur. Quantity can be -1 which indicates a default discount. Else the quantity must be grater than zero</param>
        /// <param name="di">Discount object</param>
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

        /// <summary>
        /// Add an array of discounts
        /// </summary>
        /// <param name="quantity">Quantity level for discount to occur. Quantity can be -1 which indicates a default discount. Else the quantity must be grater than zero</param>
        /// <param name="di">Discount object</param>
        public void Add((int quantity, DiscountInfo di)[] arr)
        {
            foreach (var i in arr)
            {
                Add(i.quantity, i.di);
            }
        }

        /// <summary>
        /// For a given quantity, determine what the discount should be.
        /// </summary>
        /// <returns>A discount object</returns>
        /// <param name="quantity">Quantity that has been sold. Must be greater than zero.</param>
        public DiscountInfo GetDiscount(int quantity)
        {
            Debug.Assert(quantity > 0);

            if (quantity <= 0)
                throw new ArgumentException("Must be greater than zero", nameof(quantity));
            
            var d = Discounts.Where((arg) => (arg.Key <= quantity)); // find the applicable discounts 
            if (!d.Any())
            {
                // We didn't find any discounts. Return NO DISCOUNT
                return new DiscountInfo(DiscountInfo.DiscountTypeFlag.None, null, null);
            }

            var k = d.Max((arg) => arg.Key); // get the largest applicable discount
            return d.First((arg) => arg.Key == k).Value;
        }
    }
}
