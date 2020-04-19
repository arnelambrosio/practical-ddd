using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingEngine.Api.DTOs.Request
{
    public class RegisterUser
    {
        [Required(ErrorMessage ="Username is required.")]
        [MinLength(3,ErrorMessage ="Username must be atleast 3 character.")]
        public string Username { get; set; }
    }
}
