
using DAL.Models;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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

        public async Task AddGamePlatform(Guid GameId, Guid PlatformId)
        {
            await dbSet.AddAsync(new GamePlatform { GameId = GameId, PlatformId = PlatformId });
        }

        public async Task<IEnumerable<Guid>> GetGameGuidsByPlatformId(Guid platformId)
        {
            return await dbSet.Where(x => x.PlatformId == platformId).Select(x => x.GameId).ToListAsync();
        }

        public async Task<IEnumerable<Guid>> GetPlatformGuidsByGameGuidId(Guid gameId)
        {
            return await dbSet.Where(x => x.GameId == gameId).Select(x => x.PlatformId).ToListAsync();
        }

        public async Task Update(Guid gameGuid, List<Guid> platformGuids)
        {
            foreach (var item in dbSet.Where(x => x.GameId == gameGuid))
            {
                context.Remove(item);
            }
            foreach (var item in platformGuids)
            {
                dbSet.Add(new GamePlatform { GameId = gameGuid, PlatformId = item });
            }

        }
    }
}
