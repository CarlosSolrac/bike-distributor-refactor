using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BikeDistributor
{
    public class DiscountInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DiscountTypeFlag { Percentage, FlatDiscount, Expression, None }

        public DiscountInfo(DiscountTypeFlag discountType, decimal? discount, string expression)
        {
            Debug.Assert(
                (discountType == DiscountTypeFlag.None && discount == null && expression == null) ||
                (discountType == DiscountTypeFlag.FlatDiscount && discount != null && discount.HasValue && 0 < discount) ||
                (discountType == DiscountTypeFlag.Percentage && discount != null && discount.HasValue && 0 < discount && discount < 1) ||
                (discountType == DiscountTypeFlag.Expression && discount == null && !string.IsNullOrEmpty(expression) && !string.IsNullOrWhiteSpace(expression))
            );


            switch (discountType)
            {
                case DiscountTypeFlag.None:
                    if (discount != null)
                        throw new ArgumentException("The value must be null", nameof(discount));

                    if (expression != null)
                        throw new ArgumentException("The value must be null", nameof(expression));
                    break;

                case DiscountTypeFlag.Percentage:
                case DiscountTypeFlag.FlatDiscount:
                    if (discount == null || !discount.HasValue)
                        throw new ArgumentException("The value cannot be null", nameof(discount));

                    if (discount.Value <= 0)
                        throw new ArgumentException("The value must be greater than zero", nameof(discount));

                    if (discountType == DiscountTypeFlag.Percentage && discount.Value >= 1)
                        throw new ArgumentException("The value must be less than 1", nameof(discount));
                    break;

                case DiscountTypeFlag.Expression:
                    if (discount != null)
                        throw new ArgumentException("The value must be null", nameof(discount));

                    // specifically test for null so we can provide more specific error messages
                    if (expression == null)
                        throw new ArgumentException("The expression cannot be null", nameof(expression));

                    expression = expression.Trim();

                    if (string.IsNullOrEmpty(expression))
                        throw new ArgumentException("The expression cannot be an empty string", nameof(expression));
                    break;
            }

            DiscountType = discountType;
            Discount = discount;
            Expression = expression;
        }
            
        public DiscountTypeFlag DiscountType { get; set; }
        public decimal? Discount { get; set; }
        public string Expression { get; set; }
    }
}
