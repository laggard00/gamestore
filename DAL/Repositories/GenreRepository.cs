using DAL.Interfaces;
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
    public class GenreRepository : IGenreRepository
    {
        protected readonly GameStoreDbContext context;
        private readonly DbSet<GenreEntity> dbSet;
        public GenreRepository(GameStoreDbContext context)
        {
            this.context = context;
            dbSet = context.Genres;
        }
        public async Task AddAsync(GenreEntity entity)
        {
            dbSet.Add(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(GenreEntity entity)
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

        public async Task<IEnumerable<GenreEntity>> GetAllAsync()
        {
           
            return await dbSet.ToListAsync();
        }

        public async Task<GenreEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(GenreEntity entity)
        {
            dbSet.Update(entity);
        }
    }
}
