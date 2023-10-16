using AutoMapper;
using BLL.AutoMapper;
using BLL.DTO;
using BLL.Services;
using Bogus;
using Castle.Core.Resource;
using FluentAssertions;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.ServiceTests
{
    public class AdminGenreServiceTests
    {

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        [Fact]
        public async Task AdminGenreService_GetAll_Returns_AllGenres()
        {
            //arrange
            var expected = GetGenreDTOs();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(x => x.GenreRepository.GetAllAsync())
                .ReturnsAsync(GetGenreEntities().AsEnumerable());

            var _adminGenreService = new AdminGenreService(mockUnitOfWork.Object, CreateMapperProfile());

            //act
            var actual = await _adminGenreService.GetAllAsync();

            //assert
            actual.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task AdminGenreService_GetById_Returns_GenreDTO()
        {
            //arrange
            var expected = GetGenreDTOs().First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.GenreRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetGenreEntities().First());

            var genreService = new AdminGenreService(mockUnitOfWork.Object, CreateMapperProfile());

            //act
            var actual = await genreService.GetByIdAsync(1);

            //assert
            actual.Should().BeEquivalentTo(expected);


        }

        [Fact]
        public async Task AdminGenreService_AddAsync_AddsModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.GenreRepository.AddAsync(It.IsAny<GenreEntity>()));

            var genreService = new AdminGenreService(mockUnitOfWork.Object, CreateMapperProfile());
            var genre = GetGenreDTOs().First();

            //act
            await genreService.AddAsync(genre);

            //assert
            mockUnitOfWork.Verify(x => x.GenreRepository.AddAsync(It.Is<GenreEntity>(x =>
                            x.Id == genre.Id && x.GenreName == genre.Name)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task AdminGenreService_DeleteByIdAsync_DeletesGenre(int id)
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.GenreRepository.DeleteByIdAsync(It.IsAny<int>()));
            var genreService = new AdminGenreService(mockUnitOfWork.Object,CreateMapperProfile());

            //act
            await genreService.DeleteAsync(id);

            //assert
            mockUnitOfWork.Verify(x => x.GenreRepository.DeleteByIdAsync(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());

        }

        [Fact]
        public async Task AdminGameService_UpdateAsync_UpdatesGenre()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.GenreRepository.Update(It.IsAny<GenreEntity>()));
            

            var genreService = new AdminGenreService(mockUnitOfWork.Object, CreateMapperProfile());
            var genre = GetGenreDTOs().First();

            //act
            await genreService.UpdateAsync(genre);

            //assert
            mockUnitOfWork.Verify(x => x.GenreRepository.Update(It.Is<GenreEntity>(x =>
                x.Id == genre.Id && x.GenreName == genre.Name)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }



















        public static List<GenreDTO> GetGenreDTOs()
        {
            return new List<GenreDTO> { new GenreDTO { Id=1, Name=$"name"},
                                        new GenreDTO { Id=2, Name=$"name"},
                                        new GenreDTO { Id=3, Name=$"name"},
                                        new GenreDTO { Id=4, Name=$"name"},
                                        new GenreDTO { Id=5, Name=$"name"},
                                        new GenreDTO { Id=6, Name=$"name"},
                                        new GenreDTO { Id=7, Name=$"name"}
            };
        }
        public static List<GenreEntity> GetGenreEntities()
        {
            return new List<GenreEntity> { new GenreEntity { Id=1, GenreName=$"name"},
                                        new GenreEntity { Id=2, GenreName=$"name"},
                                        new GenreEntity { Id=3, GenreName=$"name"},
                                        new GenreEntity { Id=4, GenreName=$"name"},
                                        new GenreEntity { Id=5, GenreName=$"name"},
                                        new GenreEntity { Id=6, GenreName=$"name"},
                                        new GenreEntity { Id=7, GenreName=$"name"}
            };
        }
    }
}
