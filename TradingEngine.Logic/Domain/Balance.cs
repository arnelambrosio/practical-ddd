using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingEngine.Logic.Common;
using TradingEngine.Logic.Exceptions;
using TradingEngine.Logic.SharedKernel;

namespace TradingEngine.Logic.Domain
{
    public class Balance : Entity
    {
        private Dictionary<Currency, decimal> Currencies = new Dictionary<Currency, decimal>();

        public virtual void AddMoney(Money money)
        {
            if (Currencies.ContainsKey(money.Currency))
            {
                Currencies[money.Currency] = (Currencies.GetValueOrDefault(money.Currency) + money.Amount);
            }
            else
            {
                Currencies.Add(money.Currency, money.Amount);
            }
        }

        public virtual void ChargeMoney(Money money)
        {
            ValidateIfEnoughMoneyInBalance(money);
            Currencies[money.Currency] = Currencies.GetValueOrDefault(money.Currency) - money.Amount;
        }

        public void Exchange(Money money, Currency to)
        {
            ValidateIfEnoughMoneyInBalance(money);

            decimal ratioBetweenCurrencies = Math.Round(money.Currency.Ratio / to.Ratio, 2);
            AddMoney(new Money(to, (decimal)Math.Round(money.Amount * ratioBetweenCurrencies * 100) / 100));
            ChargeMoney(money);
        }

        public IList<Money> GetAllMoney()
        {
            return Currencies.Select(a => new Money(a.Key, a.Value)).ToList();
        }

        private void ValidateIfEnoughMoneyInBalance(Money money)
        {
            decimal? moneyInBalance = Currencies.GetValueOrDefault(money.Currency);
            if (moneyInBalance == null)
            {
                throw new CurrencyNotFoundException();
            }
            else if (moneyInBalance < money.Amount)
            {
                throw new InsufficientBalanceException();
            }

        }

        public override string ToString()
        {
            return $"Balance: Currencies = {Currencies}";
        }


    }
}
