using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingEngine.Api.DTOs.Request;
using TradingEngine.Api.DTOs.Response;
using TradingEngine.Api.Extensions;
using TradingEngine.Logic.Common;
using TradingEngine.Logic.Domain;
using TradingEngine.Logic.Domain.Currencies;
using TradingEngine.Logic.Domain.User;
using TradingEngine.Logic.SharedKernel;

namespace TradingEngine.Api.Controllers
{
    [Route("/v1/api/user")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;
        public UserController(
            IUserRepository userRepository
            , IMapper mapper
            ,IRepository<Currency> currencyRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _currencyRepository = currencyRepository;
        }

        [Route("{id}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> AddMoneyToBalance([FromRoute] int id, [FromBody]AddMoneyToBalance money)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var currency = await _currencyRepository.GetAsync(money.CurrencyId);

            var moneyToAdd = new Money(currency, money.Amount);

            var user = await _userRepository.GetByIdIncludingBalanceAsync(id);
            user.Balance.AddMoney(moneyToAdd);

            var succeeded = await _userRepository.UpdateBalanceAsync(user);

            if (!succeeded)
                return BadRequest("An Error occured while processing the request.");

            return Ok();
        }

        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterUser newUser)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                var userToRegister = new User(newUser.Username, null);
                int userId = await _userRepository.InsertAsync(userToRegister);

                return Ok();

            }
            catch (Exception e)
            {
                //log error
                return StatusCode(StatusCodes.Status500InternalServerError); ;
            }
        }

        [Route("{id}/balance")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBalance([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest("UserId is required.");

            IList<GetUserBalance> response = null;

            var userBalance = await _userRepository.GetByIdIncludingBalanceAsync(id);
            if (userBalance != null)
                response = userBalance.Balance.ToGetUserBalance();

            return Ok(response);
        }

        [Route("{id}/send-money")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendMoney([FromRoute] int id, [FromBody] SendMoney money)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                var currency = await _currencyRepository.GetAsync(money.CurrencyId);

                var sender = await _userRepository.GetByIdIncludingBalanceAsync(id);
                sender.Balance.ChargeMoney(new Money(currency, money.Amount));
                await _userRepository.UpdateBalanceAsync(sender);

                var receiver = await _userRepository.GetByIdIncludingBalanceAsync(money.ToUserId);
                receiver.Balance.AddMoney(new Money(currency, money.Amount));
                await _userRepository.UpdateBalanceAsync(receiver);


                return Ok(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                //log error
                return StatusCode(StatusCodes.Status500InternalServerError); ;
            }
        }

        [Route("{id}/exchange")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExchangeMoney([FromRoute] int id, [FromBody] ExchangeMoney money)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                var fromCurrency = await _currencyRepository.GetAsync(money.FromCurrencyId);
                var toCurrency = await _currencyRepository.GetAsync(money.ToCurrencyId);

                var user = await _userRepository.GetByIdIncludingBalanceAsync(id);

                user.Balance.Exchange(new Money(fromCurrency, money.Amount), toCurrency);
                await _userRepository.UpdateBalanceAsync(user);

                return Ok(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                //log error
                return StatusCode(StatusCodes.Status500InternalServerError); ;

            }


        }
    }
}