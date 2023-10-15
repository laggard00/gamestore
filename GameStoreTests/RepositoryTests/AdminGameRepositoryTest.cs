using DAL.Models;
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
    public  class AdminGameRepositoryTest
    {
        private async Task<GameStoreDbContext> GetGameStoreDbContext() 
        {
            var options = new DbContextOptionsBuilder<GameStoreDbContext>()
                              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                              .Options;
            

            var _context = new GameStoreDbContext(options);

            _context.Database.EnsureCreated();

            if (await _context.Games.CountAsync() <= 0) 
            {
                for (int i = 0; i < 10; i++)
                {
                    
                    _context.Games.Add(new GameStore_DAL.Models.GameEntity { Id = 1+i, Name = "testy", Description = "testtt", GenreId = 1, GameAlias=""});
                    
                    await _context.SaveChangesAsync();
                }
            }
            return _context;
        }

        [Fact]

        public async void GamesRepository_GetAllAsync_ReturnsGames() 
        {
            //arrange
            var _context = await GetGameStoreDbContext();
            var gamesRepository = new GamesRepository(_context);

            //act

            var result = await gamesRepository.GetAllAsync();

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<GameEntity>));

        }

        [Fact]

        public async void GamesRepository_GetByIdAsync_ReturnsGameById()
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
            Assert.Equal(result, _context.Games.Find(gameId));

        }

        [Fact]

        public async void GamesRepository_AddAsync_UpdatesDB()
        {
            //arrange
            var gameId = 222;
            var game = new GameEntity { Id = 222, Name = "s", Description = "desc", GameAlias = "" };
            var _context = await GetGameStoreDbContext();
            var gamesRepository = new GamesRepository(_context);

            //act

             gamesRepository.Update(game);

            //assert
            
            Assert.Equal(game, _context.Games.Find(gameId));

        }

        [Fact]

        public async void GamesRepository_Delete_DeletesFromDb()
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

        public async void GamesRepository_DeleteById_DeletesFromDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var game = _context.Games.First();
           
            
            var gamesRepository = new GamesRepository(_context);

            //act


            await gamesRepository.DeleteByIdAsync(game.Id);
            var game2 = _context.Games.Count();

            //assert

            Assert.DoesNotContain(game, _context.Games);

        }
    }
}
