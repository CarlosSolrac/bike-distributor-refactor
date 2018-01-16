using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;


namespace BikeDistributor
{
    /// <summary>
    /// Holds the properties of a bike.
    /// </summary>
    /// Design: Separate the Bike properties and discount information.
    /// 
    /// Skipping documenting this class since it was provided as part of the initial code
    public class Bike
    {
        public Bike(string brand, string model, int price)
        {
            Brand = brand;
            Model = model;
            Price = price;
        }

        public Bike(string brand, string model, int price, BikeDiscounts discounts)
            : this(brand, model, price)
        {
            DiscountObj = discounts;
        }

        /// <summary>
        /// Gets the discount corresponding to the quantity sold
        /// </summary>
        /// <returns>The discount.</returns>
        /// <param name="quantity">Quantity must be greater than zero</param>
        public DiscountInfo GetDiscount(int quantity)
        {
            Debug.Assert(quantity > 0);

            if (quantity <= 0)
                throw new ArgumentException("Must be greater than zero", nameof(quantity));

            // If no discount object just return zero discount
            if (DiscountObj is null) 
                return DiscountInfo.CreateDiscountZeroAmount();
            
            return DiscountObj.GetDiscount(quantity);
        }

        public string Brand { get; private set; }
        public string Model { get; private set; }
        public decimal Price { get; private set; }
        public BikeDiscounts DiscountObj { get; private set; }
    }
}
