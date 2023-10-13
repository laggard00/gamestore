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
        private readonly DbSet<Genre> dbSet;
        public GenreRepository(GameStoreDbContext context)
        {
            this.context = context;
            dbSet = context.Genres;
        }
        public async Task AddAsync(Genre entity)
        {
            dbSet.Add(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Genre entity)
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

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
           
            return await dbSet.ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Update(Genre entity)
        {
            dbSet.Update(entity);
        }
    }
}
