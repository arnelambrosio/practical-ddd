using System;
using System.Collections.Generic;
using System.Text;
using TradingEngine.Logic.Common;
using TradingEngine.Logic.Domain;

namespace TradingEngine.Logic.SharedKernel
{
    public class Money : ValueObject<Money>
    {
        public Currency Currency { get; }
        public decimal Amount { get; }

        public Money(Currency currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }

        protected override bool EqualsCore(Money other)
        {
            return Currency == other.Currency
                && Amount == other.Amount;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                return (Currency.GetHashCode() * 397) ^ (int)Amount;
            }
        }
    }
}
