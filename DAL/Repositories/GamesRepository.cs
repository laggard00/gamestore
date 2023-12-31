using Castle.Core.Internal;
using DAL.Exceptions;

using DAL.Models;
using FluentAssertions.Execution;
using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameStore_DAL.Repositories
{
    public class GamesRepository 
    {
        protected readonly GameStoreDbContext context;
        private readonly DbSet<Game> dbSet;
        public GamesRepository(GameStoreDbContext context)
        {
            this.context = context;
            dbSet = context.Games;
        }

        public async Task<Game> AddAsync(Game entity)
        {
            var addedEntity = await context.Set<Game>().AddAsync(entity);
            return addedEntity.Entity;
        }

        public void Delete(Game entity)
        {
            if (dbSet.Contains(entity))
            {
                dbSet.Remove(entity);
                context.SaveChanges();
            }
        }

        public Task DeleteByKeyAsync(string key)
        {
            context.Remove(dbSet.Where(x => x.Key == key));
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return dbSet;
        }
        public async Task<IEnumerable<Game>> GetAllByGameGuids(IEnumerable<Guid> GameGuids) 
        {
            return await dbSet.Where(x => GameGuids.Contains(x.Id)).ToListAsync();
        }
        


        public async Task<Game> GetByIdAsync(Guid id)
        {
            var gameById = dbSet.Where(x => x.Id == id).FirstOrDefault();
            return gameById;
        }

        public async Task<Game> GetGameByAlias(string alias)
        {
            var gameByAlias = dbSet.Where(x => x.Key == alias).SingleOrDefault();

            return gameByAlias;
        }

        

        public async Task Update(Game entity)
        {

            var gameEntity = await dbSet.FindAsync(entity.Id);
            if (gameEntity == null)
            {
                throw new Exception("Game not found");
            }
            gameEntity.Name = entity.Name;
            gameEntity.Description = entity.Description;
            gameEntity.Key = entity.Key;
            gameEntity.Price = entity.Price;
            gameEntity.UnitInStock = entity.UnitInStock;
            gameEntity.Discount = entity.Discount;
            gameEntity.PublisherId = entity.PublisherId;

            context.Entry(gameEntity).State = EntityState.Modified;

        }

        public static object CheckUpdateObject(object originalObj, object updateObj)
        {
            foreach (var property in updateObj.GetType().GetProperties())
            {
                if (property.GetValue(updateObj, null) == null)
                {
                    property.SetValue(updateObj, originalObj.GetType().GetProperty(property.Name)
                    .GetValue(originalObj, null));
                }
            }
            return updateObj;
        }

        public async Task<IEnumerable<Game>> GetAllGamesWithSamePublisher(Guid id)
        {
            return dbSet.Where(x => x.PublisherId == id).ToList();
        }
    }
}
