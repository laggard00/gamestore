﻿using Castle.Core.Internal;
using DAL.Exceptions;

using DAL.Models;
using FluentAssertions.Execution;
using GameStore.DAL.Filters;
using GameStore.DAL.Models;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameStore_DAL.Repositories {
    public class GamesRepository : IGamesRepository {
        protected readonly GameStoreDbContext context;
        private readonly DbSet<Game> dbSet;
        public GamesRepository(GameStoreDbContext context) {
            this.context = context;
            dbSet = context.Games;
        }

        public async Task<Game> AddAsync(Game entity) {
            var addedEntity = await context.Set<Game>().AddAsync(entity);
            return addedEntity.Entity;
        }

        public void Delete(Game entity) {
            if (dbSet.Contains(entity)) {
                dbSet.Remove(entity);
            }
        }

        public async Task DeleteByKeyAsync(string key) {
            var game = await dbSet.SingleOrDefaultAsync(x => x.Key == key);
            context.Remove(game);

        }

        public async Task<IEnumerable<Game>> GetAllAsync(GameFilter filters) {

            var query = await BuildQuery(filters);
            var games = await query.ToListAsync();
            return games;
        }
        public async Task<IEnumerable<Game>> GetAllByGameGuids(IEnumerable<Guid> GameGuids) {
            return await dbSet.Where(x => GameGuids.Contains(x.Id)).ToListAsync();
        }

        public async Task<Game> GetByIdAsync(Guid id) {
            var gameById = await dbSet.SingleOrDefaultAsync(x => x.Id == id);
            return gameById;
        }

        public async Task<Game> GetGameByAlias(string alias) {
            var gameByAlias = await dbSet.SingleOrDefaultAsync(x => x.Key == alias);
            return gameByAlias;
        }

        public async Task Update(Game entity) {
            context.Update(entity);
        }

        public async Task<IEnumerable<Game>> GetAllGamesWithSamePublisher(Guid id) {
            return await dbSet.Where(x => x.PublisherId == id).ToListAsync();
        }

        public async Task<IQueryable<Game>> BuildQuery(GameFilter filter) {
            IQueryable<Game> query = context.Games;

            if (filter.genres != null && filter.genres.Count > 0) {
                var gameIds = context.GameGenre.Where(x => filter.genres.Contains(x.GenreId)).Select(x => x.GameId).ToList();
                query = query.Where(game => gameIds.Contains(game.Id));
            }
            if (filter.platforms != null && filter.platforms.Count > 0) {
                var gameIds = context.GamePlatforms.Where(x => filter.platforms.Contains(x.PlatformId)).Select(x => x.GameId).ToList();
                query = query.Where(game => gameIds.Contains(game.Id));
            }
            if (filter.publishers != null && filter.publishers.Count > 0) {

                query = query.Where(game => filter.publishers.Contains(game.PublisherId));

            }
            if (!string.IsNullOrEmpty(filter.name)) {
                query = query.Where(game => game.Name == filter.name);
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

            if (filter.maxPrice != null && filter.maxPrice >= 0)
                query = query.Where(game => game.Price <= filter.maxPrice);

            if (filter.page != null && filter.pageCount != null && int.TryParse(filter.pageCount, out int pageCnt)) {
                query = query.Skip((filter.page.Value - 1) * int.Parse(filter.pageCount)).Take(int.Parse(filter.pageCount));
            }

            if (!string.IsNullOrEmpty(filter.sort)) {
                switch (filter.sort.ToLower()) {
                    case "most popular": break;
                    case "most commented": query = query.OrderByDescending(game => context.Comments.Where(comments => comments.GameId == game.Id).Count()); break;
                    case "price asc": query = query.OrderBy(game => game.Price); break;
                    case "price desc": query = query.OrderByDescending(game => game.Price); break;
                    case "new": //query = query.Reverse();
                        break;

                }
            }
            return query;

        }

        public async Task UpdateUnitInStockFromCart(Guid productId, int quantity) {
            var product = await context.Games.FindAsync(productId);
            product.UnitInStock -= quantity;

        }

        public bool UnitInStockIsLargerThanOrder(IEnumerable<OrderGame> userCart) {
            foreach (var game in userCart) {
                var product = context.Games.AsNoTracking().Where(x => x.Id == game.ProductId).SingleOrDefault();
                if (product.UnitInStock <= game.Quantity) { return false; }

            }
            return true;
        }

        public async Task<bool> IsUnique(string key) {
            return !context.Games.AsNoTracking().Any(x => x.Key == key);
        }

        public async Task<int> GetGameCount(GameFilter filter) {
            var temporary = filter.page;
            filter.page = null;
            var query = await BuildQuery(filter);
            filter.page = temporary;
            return query.Count();
        }
    }
}
