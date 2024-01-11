
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

        }

        public void Delete(GenreEntity entity)
        {
            if (dbSet.Contains(entity))
            {
                dbSet.Remove(entity);     
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

        public async Task<IEnumerable<GenreEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
        public async Task<IEnumerable<GenreEntity>> GetAllByParentGenreAsync(Guid parentId)
        {
            return await dbSet.Where(x => x.ParentGenreId == parentId).ToListAsync();
        }
        public async Task<bool> CheckIfGenreGuidsExist(IEnumerable<Guid> Guids)
        {
            foreach (var id in Guids)
            {
                var exists = await context.Genres.AnyAsync(genre => genre.Id == id);

                if (!exists)
                    return false;
            }
            return true;
        }
        public async Task<GenreEntity> GetByIdAsync(Guid id)
        {
            return await dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public void Update(GenreEntity entity)
        {
            dbSet.Update(entity);
        }

        public async Task<IEnumerable<GenreEntity>> GetAllByGenreGuids(IEnumerable<Guid> GenreGuids)
        {
            return await dbSet.Where(x => GenreGuids.Contains(x.Id)).ToListAsync();
        }

        public async Task<bool> CheckIfGenreGuidExist(Guid Guid)
        {
            var exists = await context.Genres.AnyAsync(genre => genre.Id == Guid);
            return exists;

        }

        public async Task<bool> GenreIdExistsAndNotSameAsTheParent(Guid? parentId, Guid id)
        {
            return parentId != id && await context.Genres.AnyAsync(x => x.Id == id);

        }
    }
}
