using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using FakeItEasy;
using FluentAssertions;
using GameStore_v2.Controllers.AdminControllers;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameStoreTests.AdminControllerTests
{

    public class AdminGameControllerTests
    {

        private readonly IAdminGameService _service;
        private readonly IAppCache _cache;




        public AdminGameControllerTests()
        {
            _service = A.Fake<IAdminGameService>();
            _cache = A.Fake<IAppCache>();


        }

        [Fact]
        public async void AdminGameController_Get_Games_ReturnsOK()
        {
            //arrange
            var games = A.Fake<ICollection<GameDTO>>();
            var gamesList = A.Fake<List<GameDTO>>();

            var controller = new AdminGameController(_service, _cache);

            //act

            var result = await controller.Get();


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<IEnumerable<GameDTO>>));

        }

        [Fact]

        public async void AdminGameController_POST_Adds_Game_ReturnsOK()
        {
            //arrange
            var game = A.Fake<GameDTO>();
            var games = A.Fake<ICollection<GameDTO>>();
            var gamesList = A.Fake<List<GameDTO>>();
            var controller = new AdminGameController(_service, _cache);


            //act

            var result = await controller.Post(game);


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(CreatedAtActionResult));
        }

        [Fact]
        public async Task Post_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AdminGameController(_service, _cache);
            controller.ModelState.AddModelError("Error", "Invalid game model.");
            var game = new GameDTO { Id = 2, };

            // Act
            var result = await controller.Post(game);

            // Assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task AdminGameController_GetById_ReturnsOk()
        {
            // Arrange
            var controller = new AdminGameController(_service, _cache);
            
            var game = new GameDTO { Id = 2, };

            // Act
            var result = await controller.GetById(game.Id);

            // Assert
            result.Should().BeOfType(typeof(ActionResult<GameDTO>));
        }

        [Fact]
        public async Task AdminGameController_Delete_ReturnsNoContent()
        {
            // Arrange
            var controller = new AdminGameController(_service, _cache);

            var game = new GameDTO { Id = 2, };

            // Act
            var result = await controller.Remove(game);

            // Assert
            result.Should().BeOfType(typeof(NoContentResult));
        }


    }

    
}
