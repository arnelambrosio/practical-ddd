using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingEngine.Api.DTOs.Response;
using TradingEngine.Logic.Common;
using TradingEngine.Logic.Domain;
using TradingEngine.Logic.Domain.Currencies;

namespace TradingEngine.Api.Controllers
{
    [Route("v1/api/currency")]
    [Produces("application/json")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Currency> _currencyRepository;
        public CurrencyController(IMapper mapper, IRepository<Currency> currencyRepository)
        {
            _mapper = mapper;
            _currencyRepository = currencyRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<GetAllCurrency>>> Get()
        {
            var result = await _currencyRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<GetAllCurrency>>(result);
            return Ok(response);
        }
    }
}