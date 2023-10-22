using Castle.Core.Internal;
using DAL.Exceptions;
using DAL.Migrations;
using DAL.Models;
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
    public class GamesRepository : IGamesRepository
    {
        protected readonly GameStoreDbContext context;
        private readonly DbSet<GameEntity> dbSet;
        public GamesRepository(GameStoreDbContext context)
        {
            this.context = context;
            dbSet = context.Games;
        }

        public async Task AddAsync(GameEntity entity)
        {
           
            dbSet.Add(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(GameEntity entity)
        {
            if (dbSet.Contains(entity))
            {
                dbSet.Remove(entity);
                context.SaveChanges();
            }
        }

        public Task DeleteByIdAsync(int id)
        {
            var find = dbSet.Find(id);
            if (find != null)
            {
                dbSet.Remove(find);
                
                
            }
             return Task.CompletedTask;
        }

        public async Task<IEnumerable<GameEntity>> GetAllAsync()
        {
            var a = await dbSet.Include(x => x.GameGenres).Include(x=> x.GamePlatforms).ToListAsync();

            if (a.IsNullOrEmpty()) { throw new DatabaseEmptyException("Database is empty"); }

            return a;  
        }



        public async Task<GameEntity> GetByIdAsync(int id)
        {
            var b = dbSet.Include(x=>x.GamePlatforms).Include(x=>x.GameGenres).SingleOrDefault(x=> x.Id == id);

            return b;
        }

        public async Task<GameEntity> GetGameByAlias(string alias)
        {
            var gameByAlias = dbSet.Include(x=> x.GameGenres).Include(x=>x.GamePlatforms).SingleOrDefault(x => x.GameAlias.ToLower() == alias.ToLower());

            return gameByAlias;
        }

        public async Task<IEnumerable<GameEntity>> GetGamesByGenre(int genreId)
        {
            var games = context.GameGenre
                               .Where(x => x.GenreId == genreId)
                               .Include(x => x.Games)
                               .ThenInclude(g => g.GameGenres)
                               .ThenInclude(gg => gg.Genre)
                               .Include(x => x.Games)
                               .ThenInclude(g => g.GamePlatforms)
                               .Select(x => x.Games)
                               .ToList();




            if (games.Any()) 
            {
                return games;
            }
            else
            {
                throw new DatabaseEmptyException("No such games");
            }
           
            
        }

        public void Update(GameEntity entity)
        {
            context.GamePlatforms.Where(x => x.GameId == entity.Id)
                                 .ForEachAsync(x => context.GamePlatforms
                                 .Remove(x));
            
            context.GamePlatforms.AddRange(entity.GamePlatforms);


            context.GameGenre.Where(x => x.GameId == entity.Id)
                             .ForEachAsync(x => context.GameGenre
                             .Remove(x));

            context.GameGenre.AddRange(entity.GameGenres);


            
            dbSet.Update(entity);
           
        }
    }
}
