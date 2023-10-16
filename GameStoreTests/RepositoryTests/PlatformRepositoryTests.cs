using DAL.Repositories;
using FluentAssertions;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTests.RepositoryTests
{
    public class PlatformRepositoryTests
    {
        private async Task<GameStoreDbContext> GetGameStoreDbContext()
        {
            var options = new DbContextOptionsBuilder<GameStoreDbContext>()
                              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                              .Options;


            var _context = new GameStoreDbContext(options);

            

            if (await _context.Platforms.CountAsync() <= 0)
            {
                for (int i = 0; i < 20; i++)
                {

                    _context.Platforms.Add(new PlatformEntity { Id = i + 1, PlatformName = $"{i}name" });




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
        public async Task PlatformRepository_GetAllAsync_ReturnsAllGenres()
        {
            //arrange
            var _context = await GetGameStoreDbContext();
            var platformRepository = new PlatformRepository(_context);

            //act

            var result = await platformRepository.GetAllAsync();

            //assert
            result.Count().Should().Be(20);
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(List<PlatformEntity>));

        }

        [Fact]
        public async Task PlatformRepository_GetByIdAsync_ReturnPlatform()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var platformRepository = new PlatformRepository(_context);

            var platform = _context.Platforms.FirstOrDefault();

            //act

            var result = await platformRepository.GetByIdAsync(platform.Id);

            //assert

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(PlatformEntity));
            Assert.Equal(result, platform);

        }

        [Fact]

        public async Task PlatformRepository_AddAsync_UpdatesDB()
        {
            //arrange
            var platformId = 222;
            var platform = new PlatformEntity { Id = 222, PlatformName = "placeholder" };

            var _context = await GetGameStoreDbContext();

            var platformRepository = new PlatformRepository(_context);

            //act

            platformRepository.AddAsync(platform);

            //assert

            Assert.Equal(platform, _context.Platforms.Find(platformId));
            Assert.Contains(platform, _context.Platforms);

        }

        [Fact]

        public async Task PlatformRepository_Delete_DeletesFromDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var platform = _context.Platforms.First();

            var platformRepository = new PlatformRepository(_context);

            //act

            platformRepository.Delete(platform);

            //assert

            Assert.DoesNotContain(platform, _context.Platforms);

        }

        [Fact]

        public async Task PlatformRepository_DeleteByIdAsync_DeletesFromDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var platform= _context.Platforms.First();

            var platformRepository = new PlatformRepository(_context);

            //act

            await platformRepository.DeleteByIdAsync(platform.Id);

            _context.SaveChanges();

            //assert

            Assert.DoesNotContain(platform, _context.Platforms);

        }

        [Fact]

        public async Task PlatformRepository_Update_UpdatesDb()
        {
            //arrange
            var _context = await GetGameStoreDbContext();

            var platform = _context.Platforms.First();

            var oldplatformName = platform.PlatformName;
    
            platform.PlatformName = "UpdatedName";


            var platformRepository = new PlatformRepository(_context);

            //act


            platformRepository.Update(platform);
            _context.SaveChanges();


            //assert

            Assert.Equal(_context.Platforms.Find(platform.Id).PlatformName, platform.PlatformName);
            Assert.NotEqual(oldplatformName, _context.Platforms.Find(platform.Id).PlatformName);

        }
    }
}
