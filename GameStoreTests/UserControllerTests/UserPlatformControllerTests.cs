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
    public class UserPlatformControllerTests
    {

        private readonly IAdminPlatformService _service;
        private readonly IMemoryCache _cache;







        public UserPlatformControllerTests()
        {
            _service = A.Fake<IAdminPlatformService>();
            _cache = A.Fake<IMemoryCache>();



        }

        [Fact]
        public async void UserPlatformController_Get_Platform_ReturnsOK()
        {
            //arrange
            var games = A.Fake<ICollection<PlatformDTO>>();


            var controller = new UserPlatformController(_service,_cache);

            //act

            var result = await controller.Get();


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<IEnumerable<PlatformDTO>>));

        }

        [Fact]

        public async void UserPlatformController_POST_Adds_Platform_ReturnsOK()
        {
            //arrange
            var genre = A.Fake<PlatformDTO>();

            var controller = new UserPlatformController(_service,_cache);


            //act

            var result = await controller.Post(genre);


            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(CreatedAtActionResult));
        }

        [Fact]
        public async Task UserPlatformController_GetById_ReturnsOk()
        {
            // Arrange
            var controller = new UserPlatformController(_service,_cache);

            var genre = new PlatformDTO { Id = 2, };

            // Act
            var result = await controller.GetById(genre.Id);

            // Assert
            result.Should().BeOfType(typeof(ActionResult<PlatformDTO>));
        }

        [Fact]
        public async Task UserPlatformController_Delete_ReturnsNoContent()
        {
            // Arrange
            var controller = new UserPlatformController(_service,_cache);

            var genre = new PlatformDTO { Id = 2, };

            // Act
            var result = await controller.Delete(genre.Id);

            // Assert
            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
