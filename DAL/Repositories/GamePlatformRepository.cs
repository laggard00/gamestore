using DAL.Interfaces;
using DAL.Models;
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
    public class GamePlatformRepository : IGamePlatformRepository
    {

        protected readonly GameStoreDbContext context;
        private readonly DbSet<GamePlatform> dbSet;
        public GamePlatformRepository(GameStoreDbContext context)
        {
            this.context = context;
            dbSet = context.GamePlatforms;
        }

        public async Task<List<GamePlatform>> GetGamePlatformByPlatformId(int platformId)
        {
            var a = dbSet.Where(x => x.PlatformId == platformId).Include(x => x.Game);
            return await a.ToListAsync();
        }
    }
}
