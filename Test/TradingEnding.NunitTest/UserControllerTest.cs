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
    }
}
