using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;


namespace BikeDistributor
{
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

        public DiscountInfo GetDiscount(int quantity)
        {
            if (DiscountObj is null) 
                return new DiscountInfo(DiscountInfo.DiscountTypeFlag.None, null, null);
            
            return DiscountObj.Discount(quantity);
        }

        public string Brand { get; private set; }
        public string Model { get; private set; }
        public decimal Price { get; private set; }
        public BikeDiscounts DiscountObj { get; private set; }
    }
}
