using System;
using System.Collections.Generic;
using System.Text;
using Jint;

namespace BikeDistributor
{
    public class Line
    {
        public Line(Bike bike, int quantity)
        {
            Bike = bike;
            Quantity = quantity;
            TotalPrice = Bike.Price * quantity;

            DiscountInfo di = Bike.GetDiscount(quantity);

            // Round the numbers to 2 decimal places to make them print nicely
            switch(di.DiscountType)
            {
                case DiscountInfo.DiscountTypeFlag.FlatDiscount:
                    TotalDiscountAmount = Math.Round(di.Discount.Value * quantity, 2);
                    if (TotalDiscountAmount == 0m)
                        TotalDiscountPercentage = 0m;
                    else
                        TotalDiscountPercentage = Math.Round(TotalDiscountAmount / TotalPrice, 2);
                    break;
                case DiscountInfo.DiscountTypeFlag.Percentage:
                    TotalDiscountPercentage = Math.Round(di.Discount.Value, 2);
                    TotalDiscountAmount = Math.Round(TotalPrice * TotalDiscountPercentage, 2);
                    break;
                case DiscountInfo.DiscountTypeFlag.Expression:
                    var engine = new Engine();

                    engine.SetValue("Bike", bike);
                    engine.SetValue("Quantity", quantity);
                    engine.SetValue("TotalPrice", TotalPrice);

                    var totalDiscount = engine.Execute(di.Expression).GetCompletionValue().AsNumber();

                    TotalDiscountAmount = Math.Round(TotalPrice - (decimal)totalDiscount, 2);
                    TotalDiscountPercentage = Math.Round(TotalDiscountAmount / TotalPrice, 2);
                    break;
            }

            TotalAmount = TotalPrice - TotalDiscountAmount;
        }

        public Bike Bike { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice { get; private set; }
        public decimal TotalAmount { get; private set; }
        public decimal TotalDiscountPercentage { get; private set; }
        public decimal TotalDiscountAmount { get; private set; }
    }
}
