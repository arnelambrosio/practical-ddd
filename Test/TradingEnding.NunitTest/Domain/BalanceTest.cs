using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingEngine.Logic.Domain;
using TradingEngine.Logic.Exceptions;
using TradingEngine.Logic.SharedKernel;


namespace TradingEngine.UnitTest.Domain
{
    [TestFixture]
    public class BalanceTest
    {
        Currency _currencyInPeso;
        Currency _currencyInDollar;
        Currency _currencyInAU;
        Balance balance;
        Money addedMoney1;
        Money addedMoney2;
        Money addedMoney3;
        IList<Money> moneyInBalance;

        [SetUp]
        public void Init()
        {
            _currencyInPeso = new Currency("Php", 1.0M);
            _currencyInDollar = new Currency("USD",0.3M);
            _currencyInAU = new Currency("AUD",0.4M);

            addedMoney1 = new Money(_currencyInPeso, 500M);
            addedMoney2 = new Money(_currencyInPeso, 500M);
            addedMoney3 = new Money(_currencyInDollar, 900);

            balance = new Balance();
            balance.AddMoney(addedMoney1);
            balance.AddMoney(addedMoney2);
            balance.AddMoney(addedMoney3);
        }

        [Test]
        public void Add_money_will_persist_currency_and_amount()
        {
            moneyInBalance = balance.GetAllMoney();
            Assert.IsNotNull(moneyInBalance);
        }

        [Test]
        public void Add_money_will_increment_the_currency_amount()
        {
            var moneyInBalance = balance.GetAllMoney();

            var moneyInPhpCurrency = moneyInBalance.FirstOrDefault(c => c.Currency == _currencyInPeso);
            var totalAmount = moneyInBalance.Select(a => a.Amount).Sum();
            var amountInDollar = moneyInBalance.FirstOrDefault(c => c.Currency == _currencyInDollar).Amount;
            var amountInPeso = moneyInBalance.FirstOrDefault(c => c.Currency == _currencyInPeso).Amount;

            Assert.IsNotNull(moneyInBalance);
            Assert.IsNotNull(moneyInPhpCurrency);
            Assert.AreEqual(1900M, totalAmount);
            Assert.AreEqual(900,amountInDollar);
            Assert.AreEqual(1000,amountInPeso);
        }

        [Test]
        public void Charge_money_will_deduct_from_currency_amount()
        {
            var chargeMoney = new Money(_currencyInPeso,200M);

            balance.ChargeMoney(chargeMoney);

            moneyInBalance = balance.GetAllMoney();
            var moneyInPeso = moneyInBalance.FirstOrDefault(c => c.Currency == _currencyInPeso).Amount;

            Assert.IsNotNull(moneyInBalance);
            Assert.AreEqual(800M, moneyInPeso);
        }

        [Test]
        public void Charge_money_will_throw_exception_when_no_enough_balance()
        {
            var chargeMoney = new Money(_currencyInPeso, 2000M);

            Assert.Throws<InsufficientBalanceException>(() => balance.ChargeMoney(chargeMoney));
        }

        [Test]
        public void Exhance_money_will_increment_currency_balance()
        {
            var mybalance = new Balance();
            mybalance.AddMoney(addedMoney1);
            mybalance.AddMoney(addedMoney3);

            mybalance.Exchange(new Money(_currencyInPeso, 100), _currencyInDollar);
            moneyInBalance = mybalance.GetAllMoney();
            var moneyInPeso = moneyInBalance.FirstOrDefault(c => c.Currency == _currencyInPeso).Amount;
            var moneyInDollar = moneyInBalance.FirstOrDefault(c => c.Currency == _currencyInDollar).Amount;

            Assert.IsTrue(moneyInBalance.Count > 0);
            Assert.AreEqual(400M, moneyInPeso);
            Assert.IsTrue(moneyInDollar > 900M);
        }

        [Test]
        public void To_string_will_retun_not_null()
        {
            var balance = new Balance();

            string result = balance.ToString();

            Assert.IsNotNull(result);
        }
    }
}
