using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingEngine.Api.DTOs.Request
{
    public class SendMoney
    {
        [Required]
        public int ToUserId { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        [Required]
        public decimal Amount { get; set; }        
    }
}
