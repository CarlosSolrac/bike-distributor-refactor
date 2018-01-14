using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BikeDistributor.Test
{
    [TestClass]
    public class OrderTest
    {
        public const int OneThousand = 1000;
        public const int TwoThousand = 2000;
        public const int FiveThousand = 5000;

        private static BikeDiscounts DiscountOneThousand = new BikeDiscounts( new(int, DiscountInfo)[] { ( 20, new DiscountInfo(DiscountInfo.DiscountTypeFlag.Percentage, 0.1m, null) ) } );
        private static BikeDiscounts DiscountTwoThousand = new BikeDiscounts( new(int, DiscountInfo)[] { ( 10, new DiscountInfo(DiscountInfo.DiscountTypeFlag.Percentage, 0.2m, null)) } );
        private static BikeDiscounts DiscountFiveThousand = new BikeDiscounts( new(int, DiscountInfo)[] { ( 5, new DiscountInfo(DiscountInfo.DiscountTypeFlag.Percentage, 0.2m, null)) });
        private static BikeDiscounts FlatDiscount = new BikeDiscounts( new(int, DiscountInfo)[] { ( 1, new DiscountInfo(DiscountInfo.DiscountTypeFlag.FlatDiscount, 234.00m, null)) });
        private static BikeDiscounts MultipleDiscount = new BikeDiscounts( new(int, DiscountInfo)[] {
            ( 2, new DiscountInfo(DiscountInfo.DiscountTypeFlag.FlatDiscount, 234.00m, null)),
            ( 4, new DiscountInfo(DiscountInfo.DiscountTypeFlag.FlatDiscount, 345.67m, null)),
            ( 6, new DiscountInfo(DiscountInfo.DiscountTypeFlag.Percentage, 0.2m, null)),
            ( 8, new DiscountInfo(DiscountInfo.DiscountTypeFlag.Percentage, 0.3m, null))
        });

        private static string JS_Rule =
@"
var res = 0;
switch(Bike.Model)
{
    case ""Defy 1"":
        if (Quantity >= 20) res = 0.2;
        break;
    case ""Venge Elite"":
        if (Quantity >= 10) res = 0.3;
        break;
    case ""S-Works Venge Dura-Ace"":
        if (Quantity >= 5) res = 0.4;
        break;
    case ""Flat S-Works Venge Dura-Ace"":
        if (Quantity >= 7) res = 234.56;
        break;
}
res;
";

        private static BikeDiscounts JSDiscount = new BikeDiscounts(new(int, DiscountInfo)[] { (-1, new DiscountInfo(DiscountInfo.DiscountTypeFlag.Expression, null, JS_Rule)) });

        private readonly static Bike Defy = new Bike("Giant", "Defy 1", OneThousand, DiscountOneThousand);
        private readonly static Bike Defy0Disc = new Bike("Giant", "Defy 1 Zero", OneThousand);
        private readonly static Bike Elite = new Bike("Specialized", "Venge Elite", TwoThousand, DiscountTwoThousand);
        private readonly static Bike DuraAce = new Bike("Specialized", "S-Works Venge Dura-Ace", FiveThousand, DiscountFiveThousand);
        private readonly static Bike DuraAceFlatDisc = new Bike("Specialized", "Flat S-Works Venge Dura-Ace", FiveThousand, FlatDiscount);

        private readonly static Bike DefyJS = new Bike("Giant", "Defy 1", OneThousand, JSDiscount);
        private readonly static Bike EliteJS = new Bike("Specialized", "Venge Elite", TwoThousand, JSDiscount);
        private readonly static Bike DuraAceJS = new Bike("Specialized", "S-Works Venge Dura-Ace", FiveThousand, JSDiscount);
        private readonly static Bike DuraAceFlatDiscJS = new Bike("Specialized", "Flat S-Works Venge Dura-Ace", FiveThousand, JSDiscount);

        private readonly static Bike MultiDiscount = new Bike("MD", "Multiple Discounts", FiveThousand, MultipleDiscount);


        //*********************************************
        //*********************************************
        // Test with 100 for quantity and multiple invoice lines
        //*********************************************
        //*********************************************
        [TestMethod]
        public void ReceiptManyAndMultipleBikes()
        {
            List<Order> orders = new List<Order>();

            var o = new Order("Test JS Shop");
            o.AddLine(new Line(DefyJS, 1));
            o.AddLine(new Line(DefyJS, 19));
            o.AddLine(new Line(DefyJS, 20));
            o.AddLine(new Line(DefyJS, 21));
            o.AddLine(new Line(EliteJS, 1));
            o.AddLine(new Line(EliteJS, 9));
            o.AddLine(new Line(EliteJS, 10));
            o.AddLine(new Line(EliteJS, 11));
            o.AddLine(new Line(DuraAceJS, 1));
            o.AddLine(new Line(DuraAceJS, 4));
            o.AddLine(new Line(DuraAceJS, 5));
            o.AddLine(new Line(DuraAceJS, 6));
            o.AddLine(new Line(DuraAceFlatDiscJS, 1));
            o.AddLine(new Line(DuraAceFlatDiscJS, 6));
            o.AddLine(new Line(DuraAceFlatDiscJS, 7));
            o.AddLine(new Line(DuraAceFlatDiscJS, 8));
            orders.Add(o);

            o = new Order("Anywhere Bike Shop");
            o.AddLine(new Line(Defy, 1));
            o.AddLine(new Line(Defy, 19));
            o.AddLine(new Line(Defy, 20));
            o.AddLine(new Line(Defy, 21));
            o.AddLine(new Line(Defy0Disc, 100));
            o.AddLine(new Line(Elite, 1));
            o.AddLine(new Line(Elite, 9));
            o.AddLine(new Line(Elite, 10));
            o.AddLine(new Line(Elite, 11));
            o.AddLine(new Line(DuraAceFlatDisc, 1));
            o.AddLine(new Line(DuraAce, 1));
            o.AddLine(new Line(DuraAce, 4));
            o.AddLine(new Line(DuraAce, 5));
            o.AddLine(new Line(DuraAce, 6));
            orders.Add(o);

            o = new Order("Multiple Discounts Shop");
            o.AddLine(new Line(MultiDiscount, 1));
            o.AddLine(new Line(MultiDiscount, 2));
            o.AddLine(new Line(MultiDiscount, 3));
            o.AddLine(new Line(MultiDiscount, 4));
            o.AddLine(new Line(MultiDiscount, 5));
            o.AddLine(new Line(MultiDiscount, 6));
            o.AddLine(new Line(MultiDiscount, 7));
            o.AddLine(new Line(MultiDiscount, 8));
            o.AddLine(new Line(MultiDiscount, 9));
            orders.Add(o);

            Assert.AreEqual(ResultStatementManyAndMultipleBikes, new OrderRendererText(orders.ToArray()).Render());
        }

        private const string ResultStatementManyAndMultipleBikes = @"Order Receipt for Anywhere Bike Shop
	1 x Giant Defy 1 = $1,000.00
Sub-Total: $1,000.00
Tax: $72.50
Total: $1,072.50";

        [TestMethod]
        public void ReceiptOneDefy()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(Defy, 1));
            Assert.AreEqual(ResultStatementOneDefy, new OrderRendererText(order).Render());
        }

        private const string ResultStatementOneDefy = @"Order Receipt for Anywhere Bike Shop
	1 x Giant Defy 1 = $1,000.00
Sub-Total: $1,000.00
Tax: $72.50
Total: $1,072.50";

        [TestMethod]
        public void ReceiptOneElite()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(Elite, 1));
            Assert.AreEqual(ResultStatementOneElite, new OrderRendererText(order).Render());
        }

        private const string ResultStatementOneElite = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized Venge Elite = $2,000.00
Sub-Total: $2,000.00
Tax: $145.00
Total: $2,145.00";

        [TestMethod]
        public void ReceiptOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(DuraAce, 1));
            Assert.AreEqual(ResultStatementOneDuraAce, new OrderRendererText(order).Render());
        }

        private const string ResultStatementOneDuraAce = @"Order Receipt for Anywhere Bike Shop
	1 x Specialized S-Works Venge Dura-Ace = $5,000.00
Sub-Total: $5,000.00
Tax: $362.50
Total: $5,362.50";

        [TestMethod]
        public void HtmlReceiptOneDefy()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(Defy, 1));
            Assert.AreEqual(HtmlResultStatementOneDefy, new OrderRendererHtml(order).Render());
        }

        private const string HtmlResultStatementOneDefy = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Giant Defy 1 = $1,000.00</li></ul><h3>Sub-Total: $1,000.00</h3><h3>Tax: $72.50</h3><h2>Total: $1,072.50</h2></body></html>";

        [TestMethod]
        public void HtmlReceiptOneElite()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(Elite, 1));
            Assert.AreEqual(HtmlResultStatementOneElite, new OrderRendererHtml(order).Render());
        }

        private const string HtmlResultStatementOneElite = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized Venge Elite = $2,000.00</li></ul><h3>Sub-Total: $2,000.00</h3><h3>Tax: $145.00</h3><h2>Total: $2,145.00</h2></body></html>";

        [TestMethod]
        public void HtmlReceiptOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(DuraAce, 1));
            Assert.AreEqual(HtmlResultStatementOneDuraAce, new OrderRendererHtml(order).Render());
        }

        private const string HtmlResultStatementOneDuraAce = @"<html><body><h1>Order Receipt for Anywhere Bike Shop</h1><ul><li>1 x Specialized S-Works Venge Dura-Ace = $5,000.00</li></ul><h3>Sub-Total: $5,000.00</h3><h3>Tax: $362.50</h3><h2>Total: $5,362.50</h2></body></html>";

    }
}


