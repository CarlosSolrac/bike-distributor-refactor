using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BikeDistributor
{
    /// <summary>
    /// Wraps all of the dicount info into an object so it can be shared across objects.
    /// </summary>
    /// 
    /// Design: Isolate the discount storage
    public class DiscountInfo
    {
        /// <summary>
        /// Discount type flag.
        /// </summary>
        /// <list type = "bullet" >
        /// < listheader >
        ///     < term > flag </ term >
        ///     < description > description </ description >
        /// </ listheader >
        /// < item >
        ///     < term > Percentage </ term >
        ///     < description > 0.2 => 20% Valid values: (0,1] </ description >
        /// </ item >
        /// < item >
        ///     < term > FixedAmount </ term >
        ///     < description > Per unit dollar amount discount </ description >
        /// </ item >
        /// < item >
        ///     < term > Expression </ term >
        ///     < description > JavaScript expression that calculates the discount per unit dollar amount </ description >
        /// </ item >
        /// < item >
        ///     < term > None </ term >
        ///     < description > Object that represents a zero discount </ description >
        /// </ item >
        /// </ list >      
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DiscountTypeFlag { Percentage, FixedAmount, Expression, None }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BikeDistributor.DiscountInfo"/> class.
        /// </summary>
        /// <param name="discountType"> <see cref="T:BikeDistributor.DiscountInfo.DiscountTypeFlag"/> </param>
        /// <param name="discount">Represents a dollar amount or percentage depending on the discountTypeFlag</param>
        /// <param name="expression">JavaScript expression. See the Unit tests for an example of what the JavaScript should look like.</param>
        public DiscountInfo(DiscountTypeFlag discountType, decimal? discount, string expression)
        {
            // Assist with debugging
            Debug.Assert(
                (discountType == DiscountTypeFlag.None && discount == null && expression == null) ||
                (discountType == DiscountTypeFlag.FixedAmount && discount != null && discount.HasValue && 0 < discount) ||
                (discountType == DiscountTypeFlag.Percentage && discount != null && discount.HasValue && 0 < discount && discount <= 1) ||
                (discountType == DiscountTypeFlag.Expression && discount == null && !string.IsNullOrEmpty(expression) && !string.IsNullOrWhiteSpace(expression))
            );

            // Verify the values passed in are valid and perform misc cleanup.
            switch (discountType)
            {
                case DiscountTypeFlag.None:
                    if (discount != null)
                        throw new ArgumentException("The value must be null", nameof(discount));

                    if (expression != null)
                        throw new ArgumentException("The value must be null", nameof(expression));
                    break;

                case DiscountTypeFlag.Percentage:
                case DiscountTypeFlag.FixedAmount:
                    if (discount == null || !discount.HasValue)
                        throw new ArgumentException("The value cannot be null", nameof(discount));

                    if (discount.Value <= 0)
                        throw new ArgumentException("The value must be greater than zero", nameof(discount));

                    if (discountType == DiscountTypeFlag.Percentage && discount.Value > 1)
                        throw new ArgumentException("The value must be less than or equal to 1", nameof(discount));
                    break;

                case DiscountTypeFlag.Expression:
                    if (discount != null)
                        throw new ArgumentException("The value must be null", nameof(discount));

                    // specifically test for null so we can provide more specific error messages
                    if (expression == null)
                        throw new ArgumentException("The expression cannot be null", nameof(expression));

                    // Remove whitespace
                    expression = expression.Trim();

                    if (string.IsNullOrEmpty(expression))
                        throw new ArgumentException("The expression cannot be an empty string", nameof(expression));
                    break;
            }

            DiscountType = discountType;
            Discount = discount;
            Expression = expression;
        }
            
        /// <summary>
        /// Creates an object that represents ZERO discount
        /// </summary>
        /// <returns>The no discount object.</returns>
        public static DiscountInfo CreateDiscountZeroAmount()
        {
            return new DiscountInfo(DiscountInfo.DiscountTypeFlag.None, null, null);
        }

        public static DiscountInfo CreateDiscountPercentage(decimal percentage)
        {
            return new DiscountInfo(DiscountTypeFlag.Percentage, percentage, null);
        }

        public static DiscountInfo CreateDiscountFixedAmount(decimal flatAmount)
        {
            return new DiscountInfo(DiscountTypeFlag.FixedAmount, flatAmount, null);
        }

        public static DiscountInfo CreateDiscountJavaScriptExpresion(string jsexpression)
        {
            return new DiscountInfo(DiscountTypeFlag.Expression, null, jsexpression);
        }


        /// <summary>
        /// <see cref="T:BikeDistributor.DiscountInfo.DiscountTypeFlag"/>
        /// </summary>
        /// <value>The type of the discount.</value>
        public DiscountTypeFlag DiscountType { get; private set; }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        /// <value>The discount amount</value>
        public decimal? Discount { get; private set; }

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>The expression.</value>
        public string Expression { get; private set; }
    }
}
