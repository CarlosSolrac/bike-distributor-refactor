using System;
using System.Text;

namespace BikeDistributor
{
    // https://github.com/Antaris/RazorEngine
    public abstract class OrderRendererBase
    {
        public enum RenderFormat { Text, Html }

        public OrderRendererBase(Order order)
        {
            Order = order;
        }

        public abstract RenderFormat GetFormat();
        public abstract dynamic Render();

        public Order Order { get; private set; }
        public string NewLine { get; set; } = Environment.NewLine;
    }

    public class OrderRendererText : OrderRendererBase
    {
        public OrderRendererText(Order order) : base(order)
        { }

        public override RenderFormat GetFormat()
        {
            return RenderFormat.Text;
        }

        public override dynamic Render()
        {
            var result = new StringBuilder();
            result.AppendFormat("Order Receipt for {0}{1}", Order.Company, NewLine);
            foreach (var line in Order.Lines)
            {
                result.AppendFormat("\t{0} x {1} {2} = {3:C}{4}", line.Quantity, line.Bike.Brand, line.Bike.Model, line.TotalAmount, NewLine);
            }
            result.AppendFormat("Sub-Total: {0:C}{1}", Order.SubtotalOrderAmount, NewLine);
            result.AppendFormat("Tax: {0:C}{1}", Order.TaxAmount, NewLine);
            result.AppendFormat("Total: {0:C}", Order.TotalOrderAmount);
            return result.ToString();
        }
    }

    public class OrderRendererHtml : OrderRendererBase
    {
        public OrderRendererHtml(Order order) : base(order)
        { }

        public override RenderFormat GetFormat()
        {
            return RenderFormat.Html;
        }

        public override dynamic Render()
        {
            var result = new StringBuilder();
            result.AppendFormat("<html><body><h1>Order Receipt for {0}</h1>", Order.Company);

            result.Append("<ul>");
            foreach (var line in Order.Lines)
            {
                result.Append(string.Format("<li>{0} x {1} {2} = {3:C}</li>", line.Quantity, line.Bike.Brand, line.Bike.Model, line.TotalAmount));
            }
            result.Append("</ul>");

            result.AppendFormat("<h3>Sub-Total: {0:C}</h3>", Order.SubtotalOrderAmount);
            result.AppendFormat("<h3>Tax: {0:C}</h3>", Order.TaxAmount);
            result.AppendFormat("<h2>Total: {0:C}</h2>", Order.TotalOrderAmount);
            result.Append("</body></html>");
            return result.ToString();
        }

    }

}
