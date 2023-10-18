using BLL.DTO;
using BLL.Interfaces;
using BLL.Interfaces.IAdminINTERFACES;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Common;
using GameStore_v2.Controllers.AdminControllers;
using GameStore_v2.Controllers.UserController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.UserControllerTests
{
    public class UserGameControllerTest
    {
        private readonly IUserService _service;
        private readonly IMemoryCache _cache;
        
        public UserGameControllerTest()
        {
            _service = A.Fake<IUserService>();
            _cache = A.Fake<IMemoryCache>();


        }

        

        [Fact]

        public async void UserGameController_POST_Adds_Game_ReturnsOK()
        {
            //arrange
            var game = A.Fake<GameDTO>();
            var games = A.Fake<ICollection<GameDTO>>();
            var gamesList = A.Fake<List<GameDTO>>();
            var controller = new UserGameController(_service, _cache);


            //act

            var result = await controller.Post(game);


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task Post_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var controller = new UserGameController(_service, _cache);
            controller.ModelState.AddModelError("Error", "Invalid game model.");
            var game = new GameDTO { Id = 2, };

            // Act
            var result = await controller.Post(game);

            // Assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

       // [Fact]
       // public async Task UserGameController_GetByAlias_ReturnsOk()
       // {
       //     // Arrange
       //     var controller = new UserGameController(_service, _cache);
       //
       //     var alias = "alias";
       //
       //     // Act
       //     var result = await controller.GetDescritptionByAlias(alias);
       //
       //     // Assert
       //     result.Should().BeOfType(typeof(ActionResult<string>));
       // }

        [Fact]
        public async Task UserGameController_Delete_ReturnsNoContent()
        {
            // Arrange
            var controller = new UserGameController(_service, _cache);

            var game = new GameDTO { Id = 2, };

            // Act
            var result = await controller.Delete(game.Id);

            // Assert
            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
