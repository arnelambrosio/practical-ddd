using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingEngine.Api.DTOs.Request;
using TradingEngine.Api.DTOs.Response;
using TradingEngine.Logic.Domain;
using TradingEngine.Logic.SharedKernel;

namespace TradingEngine.Api.Extensions
{
    public static class ModelFactoryExtension
    {
        public static IList<GetUserBalance> ToGetUserBalance(this Balance balance)
        {
            var result = new List<GetUserBalance>();

            var moneyInBalance = balance.GetAllMoney();

            foreach (var item in moneyInBalance)
            {
                result.Add(new GetUserBalance(item.Currency.Name,item.Amount));
            }

            return result;
        }
    }
}
