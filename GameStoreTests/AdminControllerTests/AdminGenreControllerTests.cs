using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using FakeItEasy;
using FluentAssertions;
using GameStore_v2.Controllers.AdminControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.AdminControllerTests
{
    public class AdminGenreControllerTests
    {
        private readonly IAdminGenreService _service;
        




        public AdminGenreControllerTests()
        {
            _service = A.Fake<IAdminGenreService>();
            


        }

        [Fact]
        public async void AdminGenreController_Get_Genres_ReturnsOK()
        {
            //arrange
            var games = A.Fake<ICollection<GenreDTO>>();
            

            var controller = new AdminGenreController(_service);

            //act

            var result = await controller.Get();


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<IEnumerable<GenreDTO>>));

        }

        [Fact]

        public async void AdminGenreController_POST_Adds_Genre_ReturnsOK()
        {
            //arrange
            var genre = A.Fake<GenreDTO>();
            
            var controller = new AdminGenreController(_service);


            //act

            var result = await controller.Post(genre);


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(CreatedAtActionResult));
        }

        [Fact]
        public async Task AdminGenreController_GetById_ReturnsOk()
        {
            // Arrange
            var controller = new AdminGenreController(_service);

            var genre = new GenreDTO { Id = 2, };

            // Act
            var result = await controller.GetById(genre.Id);

            // Assert
            result.Should().BeOfType(typeof(ActionResult<GenreDTO>));
        }

        [Fact]
        public async Task AdminGenreController_Delete_ReturnsNoContent()
        {
            // Arrange
            var controller = new AdminGenreController(_service);

            var genre = new GenreDTO { Id = 2, };

            // Act
            var result = await controller.Delete(genre.Id);

            // Assert
            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
