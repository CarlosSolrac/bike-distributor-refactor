using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BikeDistributor
{
    // https://github.com/Antaris/RazorEngine
    public abstract class OrderRendererBase
    {
        public enum RenderFormat { Text, Html, JSON }

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

    public class OrderRendererJson : OrderRendererBase
    {
        public OrderRendererJson(Order order) : base(order)
        { }

        public OrderRendererJson(Order[] orders) : base(orders)
        { }

        public override RenderFormat GetFormat() => RenderFormat.JSON;

        public override dynamic Render()
        {
            return JsonConvert.SerializeObject(Orders, Formatting.Indented);
        }
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
                    result.AppendFormat("{0}---------------------------------------------------{0}{0}", NewLine);
                }
                isFirst = false;

                result.AppendFormat("Order Receipt for {0}{1}", o.Company, NewLine);
                foreach (var l in o.Lines)
                {
                    result.AppendFormat("\t{0} x {1} {2} = {3:C}{4}", l.Quantity, l.Bike.Brand, l.Bike.Model, l.TotalAmount, NewLine);
                }
                result.AppendFormat("Sub-Total: {0:C}{1}", o.SubtotalOrderAmount, NewLine);
                result.AppendFormat("Tax: {0:C}{1}", o.TaxAmount, NewLine);
                result.AppendFormat("Total: {0:C}{1}", o.TotalOrderAmount, NewLine);
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
            result.Append("<html><body>\n");

            foreach (Order o in Orders)
            {
                if (!isFirst)
                {
                    result.AppendLine("<hr>"); // form feed to separate pages
                }
                isFirst = false;

                result.AppendFormat("<h1>Order Receipt for {0}</h1>\n", o.Company);

                result.AppendLine("<ul>");
                foreach (var l in o.Lines)
                {
                    result.AppendFormat("\t<li>{0} x {1} {2} = {3:C}</li>\n", l.Quantity, l.Bike.Brand, l.Bike.Model, l.TotalAmount);
                }
                result.AppendLine("</ul>");

                result.AppendFormat("<h3>Sub-Total: {0:C}</h3>\n", o.SubtotalOrderAmount);
                result.AppendFormat("<h3>Tax: {0:C}</h3>\n", o.TaxAmount);
                result.AppendFormat("<h2>Total: {0:C}</h2>\n", o.TotalOrderAmount);
            }
            result.Append("</body></html>");
            return result.ToString();
        }

    }

}
