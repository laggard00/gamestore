using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using FakeItEasy;
using FluentAssertions;
using GameStore_v2.Controllers.AdminControllers;
using GameStore_v2.Controllers.UserControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.UserControllerTests
{
    public class UserGenreControllerTests
    {
        private readonly IAdminGenreService _service;
        private readonly IMemoryCache _cache;





        public UserGenreControllerTests()
        {
            _service = A.Fake<IAdminGenreService>();
            _cache= A.Fake<IMemoryCache>();


        }

        [Fact]
        public async void UserGenreController_Get_Genres_ReturnsOK()
        {
            //arrange
            var games = A.Fake<ICollection<GenreDTO>>();


            var controller = new UserGenreController(_service,_cache);

            //act

            var result = await controller.Get();


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<IEnumerable<GenreDTO>>));

        }

        [Fact]

        public async void UserGenreController_POST_Adds_Genre_ReturnsOK()
        {
            //arrange
            var genre = A.Fake<GenreDTO>();

            var controller = new UserGenreController(_service,_cache);


            //act

            var result = await controller.Post(genre);


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(CreatedAtActionResult));
        }

        [Fact]
        public async Task UserGenreController_GetById_ReturnsOk()
        {
            // Arrange
            var controller = new UserGenreController(_service,_cache);

            var genre = new GenreDTO { Id = 2, };

            // Act
            var result = await controller.GetById(genre.Id);

            // Assert
            result.Should().BeOfType(typeof(ActionResult<GenreDTO>));
        }

        [Fact]
        public async Task UserGenreController_Delete_ReturnsNoContent()
        {
            // Arrange
            var controller = new UserGenreController(_service,_cache);

            var genre = new GenreDTO { Id = 2, };

            // Act
            var result = await controller.Delete(genre.Id);

            // Assert
            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
