using System;
using System.Text;

namespace BikeDistributor
{
    public abstract class OrderRendererBase
    {
        public enum RenderFormat { Text, Html }

        public abstract RenderFormat GetFormat();
        public abstract dynamic Render(Order order);
    }

    public class OrderRendererText : OrderRendererBase
    {
        public override RenderFormat GetFormat()
        {
            return RenderFormat.Text;
        }

        public override dynamic Render(Order order)
        {
            var result = new StringBuilder();
            result.AppendFormat("Order Receipt for {0}{1}", order.Company, Environment.NewLine);
            foreach (var line in order.Lines)
            {
                result.AppendFormat("\t{0} x {1} {2} = {3:C}{4}", line.Quantity, line.Bike.Brand, line.Bike.Model, line.TotalAmount, Environment.NewLine);
            }
            result.AppendFormat("Sub-Total: {0:C}{1}", order.SubtotalOrderAmount, Environment.NewLine);
            result.AppendFormat("Tax: {0:C}{1}", order.TaxAmount, Environment.NewLine);
            result.AppendFormat("Total: {0:C}{1}", order.TotalOrderAmount, Environment.NewLine);
            return result.ToString();
        }
    }

    public abstract class OrderRendererHtml : OrderRendererBase
    {
        public override RenderFormat GetFormat()
        {
            return RenderFormat.Html;
        }

        public override dynamic Render(Order order)
        {
            var result = new StringBuilder();
            result.AppendFormat("<html><body><h1>Order Receipt for {0}</h1>", order.Company);

            result.Append("<ul>");
            foreach (var line in order.Lines)
            {
                result.Append(string.Format("<li>{0} x {1} {2} = {3:C}</li>", line.Quantity, line.Bike.Brand, line.Bike.Model, line.TotalAmount));
            }
            result.Append("</ul>");

            result.AppendFormat("<h3>Sub-Total: {0:C}</h3>", order.SubtotalOrderAmount);
            result.AppendFormat("<h3>Tax: {0:C}</h3>", order.TaxAmount);
            result.AppendFormat("<h2>Total: {0:C}</h2>", order.TotalOrderAmount);
            result.Append("</body></html>");
            return result.ToString();
        }

    }

}
