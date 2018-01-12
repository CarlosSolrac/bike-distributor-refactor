using System;

namespace BikeDistributor
{
    public class Line
    {
        public Line(Bike bike, int quantity)
        {
            Bike = bike;
            Quantity = quantity;
            TotalPrice = Bike.Price * quantity;
            DiscountPercentage = bike.Discount(quantity);
            DiscountAmount = Math.Round(TotalPrice * (1m -DiscountPercentage), 2);
            TotalAmount = TotalPrice - DiscountAmount;
        }

        public Bike Bike { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice { get; private set; }
        public decimal TotalAmount { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public decimal DiscountAmount { get; private set; }
    }
}
