using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeDistributor
{
    /// <summary>
    /// Represents an invoice or purchase order.
    /// The intent is for this to be a read only object. 
    /// Unit tests have not been written to test this.
    /// </summary>
    public class Order
    {

        public Order(string company, decimal taxRate = 0.0725M)
        {
            Company = company;
            TaxRate = taxRate;
        }

        /// <summary>
        /// Gets the company name.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; private set; }

        /// <summary>
        /// Gets the tax rate.
        /// </summary>
        /// <value>The tax rate.</value>
        public decimal TaxRate { get; private set; }

        /// <summary>
        /// Gets the subtotal order amount.
        /// </summary>
        /// <value>The subtotal order amount.</value>
        public decimal SubtotalOrderAmount { get; private set; }

        /// <summary>
        /// Gets the tax amount. This is stored vs calculated since the tax rate may change over time, so we isolate it to the order object.
        /// </summary>
        /// <value>The tax amount.</value>
        public decimal TaxAmount { get; private set; }

        /// <summary>
        /// Gets the total order amount.
        /// </summary>
        /// <value>The total order amount.</value>
        public decimal TotalOrderAmount { get; private set; }

        /// <summary>
        /// The lines of the order.
        /// </summary>
        public readonly IList<Line> Lines = new List<Line>();

        /// <summary>
        /// Adds the line to the order.
        /// </summary>
        /// <param name="line">Line.</param>
        public void AddLine(Line line)
        {
            Lines.Add(line);
            SubtotalOrderAmount += line.TotalAmount;

            // recalculate using the subtotal to avoid rounding issues.
            // The only time we should do it line by line would be if we show the tax amount by line. 
            // Then we should aggregate the amount by the lines.
            TaxAmount = Math.Round(SubtotalOrderAmount * TaxRate, 2); 

            TotalOrderAmount = SubtotalOrderAmount + TaxAmount;
        }
    }
}
