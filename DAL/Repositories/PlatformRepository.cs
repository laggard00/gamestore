using DAL.Interfaces;
using DAL.Models;
using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        protected readonly GameStoreDbContext context;
        private readonly DbSet<PlatformEntity> dbSet;
       
        public PlatformRepository(GameStoreDbContext context)
        {
            this.context = context;
            dbSet = context.Platforms;
            
        }
        public async Task AddAsync(PlatformEntity entity)
        {
            dbSet.Add(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(PlatformEntity entity)
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

        public async Task<IEnumerable<PlatformEntity>> GetAllAsync()
        {
            var a = dbSet.Include(x => x.GamePlatforms).ThenInclude(x => x.Game);
            
             return await a.ToListAsync();
        }

        public async Task<PlatformEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(PlatformEntity entity)
        {
            dbSet.Update(entity);
        }
    }
}
