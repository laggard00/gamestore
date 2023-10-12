using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task AddAsync(GameEntity entity)
        {
            dbSet.Add(entity);
            return Task.CompletedTask;
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
            return await dbSet.ToListAsync();
        }
       // public async Task<IEnumerable<GameEntity>> GetAllWithDetailsAsync()
       // {
       //     return await dbSet
       //                        .Include(c => c.Platforms)
       //                                 .Include(c => c.Genre)
       //                                            .ToListAsync();
       //
       //     
       //
       // }
        public async Task<GameEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(GameEntity entity)
        {
            dbSet.Update(entity);

        }
    }
}
