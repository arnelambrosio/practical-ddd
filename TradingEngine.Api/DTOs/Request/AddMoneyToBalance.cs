using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingEngine.Api.DTOs.Request
{
    public class AddMoneyToBalance
    {
        [Required]
        public int CurrencyId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
