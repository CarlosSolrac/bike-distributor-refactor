﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace BikeDistributor.Test
{
    [TestClass]
    public class OrderTest
    {
        public const int OneThousand = 1000;
        public const int TwoThousand = 2000;
        public const int FiveThousand = 5000;

        // Return per unit discount amount in dollars
        private static string JS_Rule =
@"
var res = 0;
switch(Bike.Model)
{
    case ""Defy 1"":
        if (Quantity >= 20) res = 0.2 * Bike.Price;
        break;
    case ""Venge Elite"":
        if (Quantity >= 10) res = 0.3 * Bike.Price;
        break;
    case ""S-Works Venge Dura-Ace"":
        if (Quantity >= 5) res = 0.4 * Bike.Price;
        break;
    case ""Flat S-Works Venge Dura-Ace"":
        if (Quantity >= 7) res = 234.56;
        break;
}
res;
";

        private static BikeDiscounts JSDiscount = new BikeDiscounts(new(int, DiscountInfo)[] { (-1, new DiscountInfo(DiscountInfo.DiscountTypeFlag.Expression, null, JS_Rule)) });
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


        [TestMethod]
        public void ReceiptMultipleOrders()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\ReceiptMultipleOrders.txt");
            Assert.AreEqual(s, new OrderRendererText(CreateOrdersMultipleReceipts().ToArray()).Render());
        }

        [TestMethod]
        public void ReceiptOneDefy()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\ReceiptOneDefy.txt");
            Assert.AreEqual(s, new OrderRendererText(CreateOrdersOneDefy()).Render());
        }

        [TestMethod]
        public void ReceiptOneElite()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\ReceiptOneElite.txt");
            Assert.AreEqual(s, new OrderRendererText(CreateOrdersOneElite()).Render());
        }

        [TestMethod]
        public void ReceiptOneDuraAce()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\ReceiptOneDuraAce.txt");
            Assert.AreEqual(s, new OrderRendererText(CreateOrdersOneDuraAce()).Render());
        }

        [TestMethod]
        public void HtmlReceiptMultipleOrders()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\HtmlReceiptMultipleOrders.txt");
            Assert.AreEqual(s, new OrderRendererHtml(CreateOrdersMultipleReceipts().ToArray()).Render());
        }

        [TestMethod]
        public void HtmlReceiptOneDefy()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\HtmlReceiptOneDefy.txt");
            Assert.AreEqual(s, new OrderRendererHtml(CreateOrdersOneDefy()).Render());
        }

        [TestMethod]
        public void HtmlReceiptOneElite()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\HtmlReceiptOneElite.txt");
            Assert.AreEqual(s, new OrderRendererHtml(CreateOrdersOneElite()).Render());
        }

        [TestMethod]
        public void HtmlReceiptOneDuraAce()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\HtmlReceiptOneDuraAce.txt");
            Assert.AreEqual(s, new OrderRendererHtml(CreateOrdersOneDuraAce()).Render());
        }


        [TestMethod]
        public void JsonReceiptMultipleOrders()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\JsonReceiptMultipleOrders.txt");
            Assert.AreEqual(s, new OrderRendererJson(CreateOrdersMultipleReceipts().ToArray()).Render());
        }

        [TestMethod]
        public void JsonReceiptOneDefy()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\JsonReceiptOneDefy.txt");
            Assert.AreEqual(s, new OrderRendererJson(CreateOrdersOneDefy()).Render());
        }

        [TestMethod]
        public void JsonReceiptOneElite()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\JsonReceiptOneElite.txt");
            Assert.AreEqual(s, new OrderRendererJson(CreateOrdersOneElite()).Render());
        }

        [TestMethod]
        public void JsonReceiptOneDuraAce()
        {
            var s = File.ReadAllText(@"..\..\ExpectedResults\JsonReceiptOneDuraAce.txt");
            Assert.AreEqual(s, new OrderRendererJson(CreateOrdersOneDuraAce()).Render());
        }

        private Order CreateOrdersOneDefy()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(Defy, 1));
            return order;
        }

        private Order CreateOrdersOneElite()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(Elite, 1));
            return order;
        }

        private Order CreateOrdersOneDuraAce()
        {
            var order = new Order("Anywhere Bike Shop");
            order.AddLine(new Line(DuraAce, 1));
            return order;
        }

        private List<Order> CreateOrdersMultipleReceipts()
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

            return orders;
        }


    }
}


