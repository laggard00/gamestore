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
            var b = context.Genres.Find(entity.GenreId);
            entity.Genre = b;
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
            var a = dbSet.Include(x => x.GamePlatforms).ThenInclude(x => x.Platform).Include(x=> x.Genre);
            return await a.ToListAsync();  
        }

        

        public async Task<GameEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<GameEntity>> GetGamesByGenre(int genreId)
        {
           var gamesByGenre=  dbSet.Where(x => x.GenreId == genreId);

           return await gamesByGenre.ToListAsync();
        }

        public void Update(GameEntity entity)
        {
            var b = context.Genres.Find(entity.GenreId);
            entity.Genre = b;
            dbSet.Update(entity);


        }
    }
}
