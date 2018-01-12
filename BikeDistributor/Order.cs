using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeDistributor
{
    public class Order
    {

        public Order(string company, decimal taxRate = 0.0725M)
        {
            Company = company;
            TaxRate = taxRate;
        }

        public string Company { get; private set; }
        public decimal TaxRate { get; private set; }
        public decimal SubtotalOrderAmount { get; private set; }
        public decimal TaxAmount { get; private set; }
        public decimal TotalOrderAmount { get; private set; }
        public readonly IList<Line> Lines = new List<Line>();

        public void AddLine(Line line)
        {
            Lines.Add(line);
            SubtotalOrderAmount += line.TotalAmount;
            TaxAmount = Math.Round(SubtotalOrderAmount * TaxRate, 2);
            TotalOrderAmount = SubtotalOrderAmount + TaxAmount;
        }
    }
}
