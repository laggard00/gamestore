using DAL.Repositories;
using FluentAssertions;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using GameStore_DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.RepositoryTests
{
    public class GenreRepositoryTests
    {
        private async Task<GameStoreDbContext> GetGameStoreDbContext()
        {

            var options = new DbContextOptionsBuilder<GameStoreDbContext>()
                              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                              .Options;


            var _context = new GameStoreDbContext(options);



            if (await _context.Genres.CountAsync() <= 0)
            {
                for (int i = 0; i < 20; i++)
                {

                    _context.Genres.Add(new GenreEntity { Id = i + 1, GenreName = $"{i}name" });




                }

                await _context.SaveChangesAsync();

            }

            return _context;
        }


        // Task<IEnumerable<TEntity>> GetAllAsync();
        //
        //
        // Task<TEntity> GetByIdAsync(int id);
        //
        // Task AddAsync(TEntity entity);
        //
        // void Delete(TEntity entity);
        //
        // Task DeleteByIdAsync(int id);
        //
        // void Update(TEntity entity);

        [Fact]
        public async Task GenreRepository_GetAllAsync_ReturnsAllGenres()
        {
            //arrange
            var _context = await GetGameStoreDbContext();
            var genreRepository = new GenreRepository(_context);

            //act

            var result = await genreRepository.GetAllAsync();

            //assert
            result.Count().Should().Be(20);
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<GenreEntity>));

        }

        [Fact]
        public async Task GenreRepository_GetByIdAsync_Genre()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var genreRepository = new GenreRepository(_context);

            var genre = _context.Genres.FirstOrDefault();

            //act

            var result = await genreRepository.GetByIdAsync(genre.Id);

            //assert

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(GenreEntity));
            Assert.Equal(result, genre);

        }

        [Fact]

        public async Task GenreRepository_AddAsync_UpdatesDB()
        {
            //arrange
            var genreId = 222;
            var genre = new GenreEntity { Id = 222, GenreName = "placeholder" };

            var _context = await GetGameStoreDbContext();

            var genreRepository = new GenreRepository(_context);

            //act

            genreRepository.AddAsync(genre);

            //assert

            Assert.Equal(genre, _context.Genres.Find(genreId));
            Assert.Contains(genre, _context.Genres);

        }

        [Fact]

        public async Task GenreRepository_Delete_DeletesFromDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var genre = _context.Genres.First();

            var genreRepository = new GenreRepository(_context);

            //act

            genreRepository.Delete(genre);

            //assert

            Assert.DoesNotContain(genre, _context.Genres);

        }

        [Fact]

        public async Task GenreRepository_DeleteByIdAsync_DeletesFromDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var genre = _context.Genres.First();

            var genreRepository = new GenreRepository(_context);

            //act

            await genreRepository.DeleteByIdAsync(genre.Id);

            _context.SaveChanges();

            //assert

            Assert.DoesNotContain(genre, _context.Genres);

        }

        [Fact]

        public async Task GenreRepository_Update_UpdatesDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var genre = _context.Genres.First();

            var oldgenreName = genre.GenreName;

            genre.GenreName = "UpdatedName";


            var genreRepository = new GenreRepository(_context);

            //act


            genreRepository.Update(genre);
            _context.SaveChanges();


            //assert

            Assert.Equal(_context.Genres.Find(genre.Id).GenreName, genre.GenreName);
            Assert.NotEqual(oldgenreName, _context.Genres.Find(genre.Id).GenreName);

        }
    }
}
