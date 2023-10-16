using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using FakeItEasy;
using FluentAssertions;
using GameStore_v2.Controllers.AdminControllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.AdminControllerTests
{
    public class AdminPlatformControllerTests
    {

        private readonly IAdminPlatformService _service;





        public AdminPlatformControllerTests()
        {
            _service = A.Fake<IAdminPlatformService>();



        }

        [Fact]
        public async void AdminPlatformController_Get_Platform_ReturnsOK()
        {
            //arrange
            var games = A.Fake<ICollection<PlatformDTO>>();


            var controller = new AdminPlatformController(_service);

            //act

            var result = await controller.Get();


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<IEnumerable<PlatformDTO>>));

        }

        [Fact]

        public async void AdminPlatformController_POST_Adds_Platform_ReturnsOK()
        {
            //arrange
            var genre = A.Fake<PlatformDTO>();

            var controller = new AdminPlatformController(_service);


            //act

            var result = await controller.Post(genre);


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(CreatedAtActionResult));
        }

        [Fact]
        public async Task AdminPlatformController_GetById_ReturnsOk()
        {
            // Arrange
            var controller = new AdminPlatformController(_service);

            var genre = new PlatformDTO { Id = 2, };

            // Act
            var result = await controller.GetById(genre.Id);

            // Assert
            result.Should().BeOfType(typeof(ActionResult<PlatformDTO>));
        }

        [Fact]
        public async Task AdminPlatformController_Delete_ReturnsNoContent()
        {
            // Arrange
            var controller = new AdminPlatformController(_service);

            var genre = new PlatformDTO { Id = 2, };

            // Act
            var result = await controller.Delete(genre.Id);

            // Assert
            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
