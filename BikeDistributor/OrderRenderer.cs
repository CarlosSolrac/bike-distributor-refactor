using System;
using System.Text;
using System.Collections.Generic;

namespace BikeDistributor
{
    // https://github.com/Antaris/RazorEngine
    public abstract class OrderRendererBase
    {
        public enum RenderFormat { Text, Html }

        public OrderRendererBase(Order order)
        {
            Orders.Add(order);
        }

        public OrderRendererBase(Order[] orders)
        {
            Orders.AddRange(orders);
        }

        public abstract RenderFormat GetFormat();
        public abstract dynamic Render();

        public List<Order> Orders { get; private set; } = new List<Order>();
        public string NewLine { get; set; } = Environment.NewLine;
    }

    public class OrderRendererText : OrderRendererBase
    {
        public OrderRendererText(Order order) : base(order)
        { }

        public OrderRendererText(Order[] orders) : base(orders)
        { }

        public override RenderFormat GetFormat() => RenderFormat.Text;

        public override dynamic Render()
        {
            bool isFirst = true;
            var result = new StringBuilder();
            foreach (Order o in Orders)
            {
                if (!isFirst)
                {
                    result.Append("\f"); // form feed to separate pages
                    isFirst = false;
                }

                result.AppendFormat("Order Receipt for {0}{1}", o.Company, NewLine);
                foreach (var l in o.Lines)
                {
                    result.AppendFormat("\t{0} x {1} {2} = {3:C}{4}", l.Quantity, l.Bike.Brand, l.Bike.Model, l.TotalAmount, NewLine);
                }
                result.AppendFormat("Sub-Total: {0:C}{1}", o.SubtotalOrderAmount, NewLine);
                result.AppendFormat("Tax: {0:C}{1}", o.TaxAmount, NewLine);
                result.AppendFormat("Total: {0:C}", o.TotalOrderAmount);
            }
            return result.ToString();
        }
    }

    public class OrderRendererHtml : OrderRendererBase
    {
        public OrderRendererHtml(Order order) : base(order)
        { }

        public OrderRendererHtml(Order[] orders) : base(orders)
        { }

        public override RenderFormat GetFormat() => RenderFormat.Html;

        public override dynamic Render()
        {
            bool isFirst = true;
            var result = new StringBuilder();
            result.Append("<html><body>");

            foreach (Order o in Orders)
            {
                if (!isFirst)
                {
                    result.Append("<hr>"); // form feed to separate pages
                    isFirst = false;
                }

                result.AppendFormat("<h1>Order Receipt for {0}</h1>", o.Company);

                result.Append("<ul>");
                foreach (var l in o.Lines)
                {
                    result.Append(string.Format("<li>{0} x {1} {2} = {3:C}</li>", l.Quantity, l.Bike.Brand, l.Bike.Model, l.TotalAmount));
                }
                result.Append("</ul>");

                result.AppendFormat("<h3>Sub-Total: {0:C}</h3>", o.SubtotalOrderAmount);
                result.AppendFormat("<h3>Tax: {0:C}</h3>", o.TaxAmount);
                result.AppendFormat("<h2>Total: {0:C}</h2>", o.TotalOrderAmount);
            }
            result.Append("</body></html>");
            return result.ToString();
        }

    }

}
