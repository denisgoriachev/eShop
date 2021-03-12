using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public const decimal MinPrice = 0;
        public const decimal MaxPrice = 99_999_999;

        public decimal Value { get; }

        public string Currency { get; }

        public Money(decimal value, string currency)
        {
            if (value < MinPrice)
                throw new DomainValidationException($"Price cannot be less then {MinPrice}.");

            if (value > MaxPrice)
                throw new DomainValidationException($"Price cannot be more then {MaxPrice}.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainValidationException("Currency cannot be null or empty");

            Value = value;
            Currency = currency;
        }

        public static Money From(decimal value, string currency = "USD")
        {
            return new Money(value, currency);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
