using Castle.Core.Internal;
using DAL.Exceptions;

using DAL.Models;
using FluentAssertions.Execution;
using GameStore.DAL.Filters;
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
using System.Xml;

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

        public async Task<IEnumerable<Game>> GetAllAsync(GameFilter filters)
        {
            var query =await BuildQuery(filters);
            var games= await query.ToListAsync();
            return games;
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

      // public static object CheckUpdateObject(object originalObj, object updateObj)
      // {
      //     foreach (var property in updateObj.GetType().GetProperties())
      //     {
      //         if (property.GetValue(updateObj, null) == null)
      //         {
      //             property.SetValue(updateObj, originalObj.GetType().GetProperty(property.Name)
      //             .GetValue(originalObj, null));
      //         }
      //     }
      //     return updateObj;
      // }

        public async Task<IEnumerable<Game>> GetAllGamesWithSamePublisher(Guid id)
        {
            return dbSet.Where(x => x.PublisherId == id).ToList();
        }

        public async Task<IQueryable<Game>> BuildQuery(GameFilter filter)
        {
            IQueryable<Game> query = context.Games;

            if (filter.genres != null && filter.genres.Count > 0)
            {
                var gameIds = context.GameGenre.Where(x => filter.genres.Contains(x.GenreId)).Select(x=>x.GameId).ToList();
                query = query.Where(game => gameIds.Contains(game.Id));
            }
            if (filter.platforms != null && filter.platforms.Count > 0)
            {
                var gameIds = context.GamePlatforms.Where(x => filter.platforms.Contains(x.PlatformId)).Select(x => x.GameId).ToList();
                query = query.Where(game => gameIds.Contains(game.Id));
            }
            if (filter.publishers != null && filter.publishers.Count > 0)
            {
                
                query = query.Where(game => filter.publishers.Contains(game.PublisherId));
                
            }
            if (!string.IsNullOrEmpty(filter.name))
            {
                query = query.Where(game => game.Name.Contains(filter.name));
            }
            //if (!string.IsNullOrEmpty(filter.datePublishing))
            //{
            //    switch (filter.datePublishing.ToLower()) 
            //    {
            //        case "last week": query =query.Where(game=> game.)
            //        case "last month":
            //        case "last year":
            //        case "2 years":
            //        case "3 years":
            //        default:break;
            //    }
            //    
            //}
            if (filter.minPrice != null && filter.minPrice >= 0)
                query = query.Where(game => game.Price >= filter.minPrice);
            
            if(filter.maxPrice!=null && filter.maxPrice >= 0)
                query=query.Where(game=> game.Price <=filter.maxPrice);
           
           if (filter.page != null && filter.pageCount != null && int.TryParse(filter.pageCount, out int pageCnt))
           {
               query = query.Skip((filter.page.Value - 1) * int.Parse(filter.pageCount)).Take(int.Parse(filter.pageCount));
           }
            if (!string.IsNullOrEmpty(filter.sort))
            {
                switch(filter.sort.ToLower())
                {
                    case "most popular":break;
                    case "most commented": query = query.OrderBy(game=> context.Comments.Where(comments=> comments.GameId==game.Id).Count()); break;
                    case "price asc":query = query.OrderBy(game => game.Price); break;
                    case "price desc": query = query.OrderByDescending(game => game.Price); break;
                    case "new": //query = query.Reverse();
                                break;
                    default:break;

                }
            }
            return query;
            
        }
    }
}
