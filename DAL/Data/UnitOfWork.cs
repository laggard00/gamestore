using GameStore_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore_DAL.Repositories;
using DAL.Interfaces;
using DAL.Repositories;

namespace GameStore_DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameStoreDbContext context;
        public UnitOfWork(GameStoreDbContext context)
        {
            this.context = context;
        }
        public IGamesRepository GamesRepository => new GamesRepository(context);

        public IGenreRepository GenreRepository => new GenreRepository(context);
        public IPlatformRepository PlatformRepository => new PlatformRepository(context);

        public IGamePlatformRepository GamePlatformRepository => new GamePlatformRepository(context);

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
