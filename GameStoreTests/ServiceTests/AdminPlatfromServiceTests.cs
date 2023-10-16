using AutoMapper;
using BLL.AutoMapper;
using BLL.DTO;
using BLL.Services;
using FluentAssertions;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.ServiceTests
{
    public class AdminPlatfromServiceTests
    {
        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        [Fact]
        public async Task AdminPlatformService_GetAll_Returns_AllPlatforms()
        {
            //arrange
            var expected = GetPlatformDTOs();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.PlatformRepository.GetAllAsync())
                .ReturnsAsync(GetPlatformEntities().AsEnumerable());

            var _adminplatformService = new AdminPlatformService(mockUnitOfWork.Object, CreateMapperProfile());

            //act
            var actual = await _adminplatformService.GetAllAsync();

            //assert
            actual.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task AdminPlatformService_GetById_Returns_PlatformDTO()
        {
            //arrange
            var expected = GetPlatformDTOs().First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.PlatformRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetPlatformEntities().First());

            var Service = new AdminPlatformService(mockUnitOfWork.Object, CreateMapperProfile());

            //act
            var actual = await Service.GetByIdAsync(1);

            //assert
            actual.Should().BeEquivalentTo(expected);


        }

        [Fact]
        public async Task AdminPlatformService_AddAsync_AddsModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.PlatformRepository.AddAsync(It.IsAny<PlatformEntity>()));

            var Service = new AdminPlatformService(mockUnitOfWork.Object, CreateMapperProfile());
            var platform = GetPlatformDTOs().First();

            //act
            await Service.AddAsync(platform);

            //assert
            mockUnitOfWork.Verify(x => x.PlatformRepository.AddAsync(It.Is<PlatformEntity>(x =>
                            x.Id == platform.Id && x.PlatformName == platform.Name)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task AdminPlatformService_DeleteByIdAsync_DeletesPlatform(int id)
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.PlatformRepository.DeleteByIdAsync(It.IsAny<int>()));
            var Service = new AdminPlatformService(mockUnitOfWork.Object, CreateMapperProfile());

            //act
            await Service.DeleteAsync(id);

            //assert
            mockUnitOfWork.Verify(x => x.PlatformRepository.DeleteByIdAsync(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());

        }

        [Fact]
        public async Task AdminPlatformervice_UpdateAsync_UpdatesPlatform()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.PlatformRepository.Update(It.IsAny<PlatformEntity>()));


            var Service = new AdminPlatformService(mockUnitOfWork.Object, CreateMapperProfile());
            var platform = GetPlatformDTOs().First();

            //act
            await Service.UpdateAsync(platform);

            //assert
            mockUnitOfWork.Verify(x => x.PlatformRepository.Update(It.Is<PlatformEntity>(x =>
                x.Id == platform.Id && x.PlatformName== platform.Name)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }



















        public static List<PlatformDTO> GetPlatformDTOs()
        {
            return new List<PlatformDTO> { new PlatformDTO { Id=1, Name=$"name"},
                                        new PlatformDTO { Id=2, Name=$"name"},
                                        new PlatformDTO { Id=3, Name=$"name"},
                                        new PlatformDTO { Id=4, Name=$"name"},
                                        new PlatformDTO { Id=5, Name=$"name"},
                                        new PlatformDTO { Id=6, Name=$"name"},
                                        new PlatformDTO { Id=7, Name=$"name"}
            };
        }
        public static List<PlatformEntity> GetPlatformEntities()
        {
            return new List<PlatformEntity> { new PlatformEntity { Id=1, PlatformName=$"name"},
                                           new PlatformEntity { Id=2, PlatformName=$"name"},
                                           new PlatformEntity { Id=3, PlatformName=$"name"},
                                           new PlatformEntity { Id=4, PlatformName=$"name"},
                                           new PlatformEntity { Id=5, PlatformName=$"name"},
                                           new PlatformEntity { Id=6, PlatformName=$"name"},
                                           new PlatformEntity { Id=7, PlatformName=$"name"}
            };
        }
    }
}
