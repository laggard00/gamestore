
using DAL.Models;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;

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
        private readonly DbSet<Platform> dbSet;

        public PlatformRepository(GameStoreDbContext context)
        {
            this.context = context;
            dbSet = context.Platforms;

        }
        public async Task AddAsync(Platform entity)
        {
            dbSet.Add(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Platform entity)
        {
            if (dbSet.Contains(entity))
            {
                dbSet.Remove(entity);
                context.SaveChanges();
            }
        }

        public Task DeleteByIdAsync(Guid id)
        {
            var find = dbSet.Find(id);
            if (find != null)
            {
                dbSet.Remove(find);

            }
            return Task.CompletedTask;
        }
        public async Task<bool> CheckIfPlatformGuidsExist(IEnumerable<Guid> Guids)
        {
            foreach (var id in Guids)
            {
                var exists = await context.Platforms.AnyAsync(genre => genre.Id == id);

                if (!exists)
                    return false;
            }
            return true;
        }
        public async Task<IEnumerable<Platform>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Platform> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(Platform entity)
        {
            dbSet.Update(entity);
        }

        public async Task<IEnumerable<Platform>> GetAllByPlatformGuids(IEnumerable<Guid> platformGuids)
        {
            return await dbSet.Where(x => platformGuids.Contains(x.Id)).ToListAsync();
        }
    }
}
