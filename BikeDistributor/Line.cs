using System;
using System.Collections.Generic;
using System.Text;
using Jint;

namespace BikeDistributor
{
    /// <summary>
    /// Corresponds to a line item in the purchase order
    /// </summary>
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
                case DiscountInfo.DiscountTypeFlag.None:
                    TotalDiscountAmount = 0m;
                    TotalDiscountPercentage = 0m;
                    break;

                case DiscountInfo.DiscountTypeFlag.FlatDiscount:
                    // Dollar amount
                    UnitDiscountAmount = Math.Round(di.Discount.Value, 2);
                    TotalDiscountAmount = Math.Round(UnitDiscountAmount * quantity, 2);
                    if (TotalDiscountAmount == 0m)
                    {
                        UnitDiscountPercentage = 0m;
                        TotalDiscountPercentage = 0m;
                    }
                    else
                    {
                        UnitDiscountPercentage = Math.Round(UnitDiscountAmount / Bike.Price, 2);
                        TotalDiscountPercentage = Math.Round(TotalDiscountAmount / TotalPrice, 2);
                    }
                    break;

                case DiscountInfo.DiscountTypeFlag.Percentage:
                    // percentage value
                    UnitDiscountPercentage = Math.Round(di.Discount.Value, 2);
                    TotalDiscountPercentage = UnitDiscountPercentage;
                    UnitDiscountAmount = Math.Round(Bike.Price * UnitDiscountPercentage, 2);
                    TotalDiscountAmount = Math.Round(TotalPrice * TotalDiscountPercentage, 2);
                    break;

                case DiscountInfo.DiscountTypeFlag.Expression:
                    // JavaScript expression
                    var engine = new Engine();

                    engine.SetValue("Bike", bike);
                    engine.SetValue("Quantity", quantity);
                    engine.SetValue("TotalPrice", TotalPrice);

                    var res = (decimal)engine.Execute(di.Expression).GetCompletionValue().AsNumber();

                    UnitDiscountAmount = Math.Round((decimal)res, 2);
                    UnitDiscountPercentage = Math.Round(UnitDiscountAmount / Bike.Price, 2);

                    TotalDiscountAmount = Math.Round(Quantity * UnitDiscountAmount, 2);
                    TotalDiscountPercentage = UnitDiscountPercentage;
                    break;
            }

            TotalAmount = TotalPrice - TotalDiscountAmount;
        }

        /// <summary>
        /// Gets the bike.
        /// </summary>
        /// <value>The bike.</value>
        public Bike Bike { get; private set; }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the total price. Quantity * Bike.Price
        /// </summary>
        /// <value>The total price.</value>
        public decimal TotalPrice { get; private set; }

        /// <summary>
        /// Gets the total amount for the order line.
        /// There are multiple ways to calculate the total amount.
        /// 
        /// TotalAmount = (Quantity * (Bike.Price - UnitDiscountAmount))
        /// TotalAmount = (Quantity * Bike.Price - TotalDiscountAmount)
        /// 
        /// TotalAmount = (Quantity * Bike.Price * (1 - UnitDiscountPercentage))
        /// TotalAmount = (Quantity * Bike.Price * (1 - TotalDiscountPercentage))
        /// </summary>
        /// <value>The total amount.</value>
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Gets the unit discount percentage per unit.
        /// </summary>
        /// <value>The unit discount percentage.</value>
        public decimal UnitDiscountPercentage { get; private set; }

        /// <summary>
        /// Gets the total discount percentage for the entire line.
        /// TotalDiscountPercentage and UnitDiscountPercentage should be the same.
        /// Any discrepancy will be due to rounding issues.
        /// </summary>
        /// <value>The total discount percentage.</value>
        public decimal TotalDiscountPercentage { get; private set; }

        /// <summary>
        /// Gets the unit discount amount in dollares.
        /// </summary>
        /// <value>The unit discount amount.</value>
        public decimal UnitDiscountAmount { get; private set; }

        /// <summary>
        /// Gets the total discount amount in dollars.
        /// TotalDiscountAmount = UnitDiscountAmount * Quantity
        /// </summary>
        /// <value>The total discount amount.</value>
        public decimal TotalDiscountAmount { get; private set; }
    }
}
