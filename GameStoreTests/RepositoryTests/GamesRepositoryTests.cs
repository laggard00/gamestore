using Bogus;
using DAL.Models;
using FluentAssertions;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using GameStore_DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.RepositoryTests
{
    public class GamesRepositoryTests
    {
        private async Task<GameStoreDbContext> GetGameStoreDbContext()
        {
            var options = new DbContextOptionsBuilder<GameStoreDbContext>()
                              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                              .Options;


            var _context = new GameStoreDbContext(options);



            if (await _context.Games.CountAsync() <= 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    _context.Platforms.Add(new PlatformEntity { Id = i + 1, PlatformName = $"{i}name" });
                    _context.Genres.Add(new GenreEntity { Id = i + 1, GenreName = $"{i}name" });
                    _context.Games.Add(new GameEntity { Id = i + 1, Name = $"{i}name", Description = $"{i}desc", GameAlias = $"{i}alias"});



                }
                await _context.SaveChangesAsync();

            }

            return _context;
        }

        //Task<IEnumerable<TEntity>> GetAllAsync();
        //
        //
        //Task<TEntity> GetByIdAsync(int id);
        //
        //Task AddAsync(TEntity entity);
        //
        //void Delete(TEntity entity);
        //
        //Task DeleteByIdAsync(int id);
        //
        //void Update(TEntity entity);

        [Fact]

        public async Task GamesRepository_GetAllAsync_ReturnsGames()
        {
            //arrange
            var _context = await GetGameStoreDbContext();
            var gamesRepository = new GamesRepository(_context);

            //act

            var result = await gamesRepository.GetAllAsync();

            //assert
            result.Count().Should().Be(20);
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<GameEntity>));

        }

        [Fact]

        public async Task GamesRepository_GetByIdAsync_ReturnsGameById()
        {
            //arrange
            var gameId = 1;
            var _context = await GetGameStoreDbContext();
            var gamesRepository = new GamesRepository(_context);

            //act

            var result = await gamesRepository.GetByIdAsync(gameId);

            //assert

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(GameEntity));
            Assert.Contains(result, _context.Games);
            Assert.Equal(result, _context.Games.Find(gameId));

        }

        [Fact]

        public async Task GamesRepository_AddAsync_UpdatesDB()
        {
            //arrange
            var gameId = 222;
            var game = new GameEntity { Id = 222, Name = "s", Description = "desc", GameAlias = "alias"};
            var _context = await GetGameStoreDbContext();
            var gamesRepository = new GamesRepository(_context);

            //act

            gamesRepository.AddAsync(game);

            //assert

            Assert.Equal(game, _context.Games.Find(gameId));
            Assert.Contains(game, _context.Games);

        }

        [Fact]

        public async Task GamesRepository_Delete_DeletesFromDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();
            var gameId = 2;
            var game = _context.Games.First();

            var gamesRepository = new GamesRepository(_context);

            //act

            gamesRepository.Delete(game);

            //assert

            Assert.DoesNotContain(game, _context.Games);

        }

        [Fact]

        public async Task GamesRepository_DeleteById_DeletesFromDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var game = _context.Games.First();


            var gamesRepository = new GamesRepository(_context);

            //act


            await gamesRepository.DeleteByIdAsync(game.Id);
            _context.SaveChanges();


            //assert

            Assert.DoesNotContain(game, _context.Games);

        }

        [Fact]

        public async Task GamesRepository_Update_UpdatesDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var game = _context.Games.First();
            var oldgameName = game.Name;
            game.Name = "UpdatedName";


            var gamesRepository = new GamesRepository(_context);

            //act


            gamesRepository.Update(game);
            _context.SaveChanges();


            //assert

            Assert.Equal(_context.Games.Find(game.Id).Name, game.Name);
            Assert.NotEqual(oldgameName, _context.Games.Find(game.Id).Name);

        }
    }
}
