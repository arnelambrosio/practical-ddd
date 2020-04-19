using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TradingEngine.Api.Controllers;
using TradingEngine.Api.DTOs.Request;
using TradingEngine.Api.DTOs.Response;
using TradingEngine.Logic.Common;
using TradingEngine.Logic.Domain;
using TradingEngine.Logic.Domain.User;
using TradingEngine.Logic.SharedKernel;

namespace TradingEngine.NunitTest
{
    [TestFixture]
    public class UserControllerTest
    {
       
        [Test]
        public async Task Register_user_should_return_ok()
        {
            var userId = 1;
            var users = new User("user101", new Balance()) { Id = userId};
            var iMapperMock = new Mock<IMapper>();
            var userRepo = new Mock<IUserRepository>();
            var currencyRepo = new Mock<IRepository<Currency>>();

            userRepo.Setup(x => x.GetAsync(userId)).ReturnsAsync(users);

            var controller = new UserController(userRepo.Object,iMapperMock.Object,currencyRepo.Object);

            var result = await controller.Register(new RegisterUser() { Username = "user101"});

            var contentResult = result as OkResult;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(StatusCodes.Status200OK,contentResult.StatusCode);
        }

        [Test]
        public async Task Register_user_should_return_bad_request_in_validation_error()
        {
            var iMapperMock = new Mock<IMapper>();
            var userRepoMock = new Mock<IUserRepository>();
            var currencyRepoMock = new Mock<IRepository<Currency>>();

            var controller = new UserController(userRepoMock.Object, iMapperMock.Object, currencyRepoMock.Object);
            controller.ModelState.AddModelError("Username","Required");

            var result = await controller.Register(new RegisterUser() { Username = ""});

            var contentResult = result as BadRequestObjectResult;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, contentResult.StatusCode);
        }

        [Test]
        public async Task User_get_balance()
        {
            var userId = 1;
            var balance = new Balance();
            balance.AddMoney(new Money(new Currency("Php",0.45M), 500));
            var user = new User("user101", balance) { Id = userId};

            var iMapperMock = new Mock<IMapper>();
            var userRepo = new Mock<IUserRepository>();
            var currencyRepo = new Mock<IRepository<Currency>>();

            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(userId)).ReturnsAsync(user);

            var controller = new UserController(userRepo.Object, iMapperMock.Object, currencyRepo.Object);

            var result = await controller.GetBalance(userId);
            var contentResult = result as OkObjectResult;
            var data = contentResult.Value as IList<GetUserBalance>;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, contentResult.StatusCode);
            Assert.IsNotNull(data);
            Assert.AreEqual(500,data.FirstOrDefault().Amount);
        }

        [Test]
        public async Task Add_money_to_balance_return_bad_response_when_error_occured()
        {
            var userId = 1;
            var currency = new Currency("Php", 0.45M) { Id = 2 };

            var balance = new Balance();
            balance.AddMoney(new Money(currency, 500));
            var user = new User("user101", balance) { Id = userId };

            var iMapperMock = new Mock<IMapper>();

            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(userId)).ReturnsAsync(user);
            userRepo.Setup(x => x.UpdateBalanceAsync(user)).ReturnsAsync(false);

            var currencyRepo = new Mock<IRepository<Currency>>();
            currencyRepo.Setup(x => x.GetAsync(2)).ReturnsAsync(currency);

            var controller = new UserController(userRepo.Object, iMapperMock.Object, currencyRepo.Object);

            var errorResult = await controller.AddMoneyToBalance(userId, new AddMoneyToBalance() { CurrencyId = 2, Amount = 500 });
            var errorContentResult = errorResult as BadRequestObjectResult;

            Assert.IsNotNull(errorResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, errorContentResult.StatusCode);
        }

        [Test]
        public async Task Add_money_to_balance_return_ok_response_when_no_error_occured()
        {
            var userId = 1;
            var currency = new Currency("Php", 0.45M) { Id = 2 };

            var balance = new Balance();
            balance.AddMoney(new Money(currency, 500));
            var user = new User("user101", balance) { Id = userId };

            var iMapperMock = new Mock<IMapper>();

            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(userId)).ReturnsAsync(user);
            userRepo.Setup(x => x.UpdateBalanceAsync(user)).ReturnsAsync(true);

            var currencyRepo = new Mock<IRepository<Currency>>();
            currencyRepo.Setup(x => x.GetAsync(2)).ReturnsAsync(currency);

            var controller = new UserController(userRepo.Object, iMapperMock.Object, currencyRepo.Object);

            var errorResult = await controller.AddMoneyToBalance(userId, new AddMoneyToBalance() { CurrencyId = 2, Amount = 500 });
            var errorContentResult = errorResult as OkResult;

            Assert.IsNotNull(errorResult);
            Assert.AreEqual(StatusCodes.Status200OK, errorContentResult.StatusCode);
        }

        [Test]
        public async Task Send_money_return_bad_response_when_validation_failed()
        {
            var userId = 1;
            var currency = new Currency("Php", 0.45M) { Id = 2 };

            var balance = new Balance();
            balance.AddMoney(new Money(currency, 500));
            var user = new User("user101", balance) { Id = userId };

            var iMapperMock = new Mock<IMapper>();

            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(userId)).ReturnsAsync(user);
            userRepo.Setup(x => x.UpdateBalanceAsync(user)).ReturnsAsync(false);

            var currencyRepo = new Mock<IRepository<Currency>>();
            currencyRepo.Setup(x => x.GetAsync(2)).ReturnsAsync(currency);

            var controller = new UserController(userRepo.Object, iMapperMock.Object, currencyRepo.Object);
            controller.ModelState.AddModelError("CurrencyId","Required");

            var errorResult = await controller.SendMoney(userId, new SendMoney() { ToUserId = 2,CurrencyId = 1,Amount = 500});
            var errorContentResult = errorResult as BadRequestObjectResult;

            Assert.IsNotNull(errorResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, errorContentResult.StatusCode);
        }

        [Test]
        public async Task Send_money_return_ok_response_when_no_error_occured()
        {
            var userId = 1;
            var currency = new Currency("Php", 0.45M) { Id = 2 };

            var balance = new Balance();
            balance.AddMoney(new Money(currency, 500));

            var user = new User("user101", balance) { Id = userId };
            var user2 = new User("user102", balance) { Id = 2 };

            var iMapperMock = new Mock<IMapper>();

            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(userId)).ReturnsAsync(user);
            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(2)).ReturnsAsync(user2);
            userRepo.Setup(x => x.UpdateBalanceAsync(user)).ReturnsAsync(true);

            var currencyRepo = new Mock<IRepository<Currency>>();
            currencyRepo.Setup(x => x.GetAsync(2)).ReturnsAsync(currency);

            var controller = new UserController(userRepo.Object, iMapperMock.Object, currencyRepo.Object);

            var result = await controller.SendMoney(userId, new SendMoney() { ToUserId = 2, CurrencyId = 2, Amount = 500 });
            var contentResult = result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, contentResult.StatusCode);
        }

        [Test]
        public async Task Exchange_money_return_bad_response_when_validation_failed()
        {
            var userId = 1;
            var currency = new Currency("Php", 0.45M) { Id = 1 };
            var currency2 = new Currency("USD", 0.45M) { Id = 2 };


            var balance = new Balance();
            balance.AddMoney(new Money(currency, 500));
            var user = new User("user101", balance) { Id = userId };

            var iMapperMock = new Mock<IMapper>();

            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(userId)).ReturnsAsync(user);
            userRepo.Setup(x => x.UpdateBalanceAsync(user)).ReturnsAsync(false);

            var currencyRepo = new Mock<IRepository<Currency>>();
            currencyRepo.Setup(x => x.GetAsync(1)).ReturnsAsync(currency);
            currencyRepo.Setup(x => x.GetAsync(2)).ReturnsAsync(currency2);

            var controller = new UserController(userRepo.Object, iMapperMock.Object, currencyRepo.Object);
            controller.ModelState.AddModelError("FromCurrencyId", "Required");

            var errorResult = await controller.ExchangeMoney(userId, new ExchangeMoney() {FromCurrencyId = 1, Amount = 50, ToCurrencyId = 2 });
            var errorContentResult = errorResult as BadRequestObjectResult;

            Assert.IsNotNull(errorResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, errorContentResult.StatusCode);
        }

        [Test]
        public async Task Exhange_money_return_ok_response_when_no_error_occured()
        {
            var userId = 1;
            var currency = new Currency("Php", 0.45M) { Id = 1 };
            var currency2 = new Currency("USD", 0.45M) { Id = 2 };

            var balance = new Balance();
            balance.AddMoney(new Money(currency, 500));

            var user = new User("user101", balance) { Id = userId };
            var user2 = new User("user102", balance) { Id = 2 };

            var iMapperMock = new Mock<IMapper>();

            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(userId)).ReturnsAsync(user);
            userRepo.Setup(x => x.GetByIdIncludingBalanceAsync(2)).ReturnsAsync(user2);
            userRepo.Setup(x => x.UpdateBalanceAsync(user)).ReturnsAsync(true);

            var currencyRepo = new Mock<IRepository<Currency>>();
            currencyRepo.Setup(x => x.GetAsync(1)).ReturnsAsync(currency);
            currencyRepo.Setup(x => x.GetAsync(2)).ReturnsAsync(currency2);


            var controller = new UserController(userRepo.Object, iMapperMock.Object, currencyRepo.Object);

            var result = await controller.ExchangeMoney(userId, new ExchangeMoney() { FromCurrencyId = 1, Amount = 50, ToCurrencyId = 2 });
            var contentResult = result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, contentResult.StatusCode);
        }

    }
}
