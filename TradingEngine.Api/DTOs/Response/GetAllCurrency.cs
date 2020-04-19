using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingEngine.Api.DTOs.Response
{
    public class GetAllCurrency
    {
        public string Name { get; set; }
        public decimal Ratio { get; set; }       
    }
}
