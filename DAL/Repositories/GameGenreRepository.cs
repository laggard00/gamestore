using DAL.Models;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories {
    public class GameGenreRepository : IGameGenreRepository {

        protected readonly GameStoreDbContext context;
        private readonly DbSet<GameGenre> dbSet;
        public GameGenreRepository(GameStoreDbContext context) {
            this.context = context;
            dbSet = context.GameGenre;
        }

        public async Task AddGameGenre(Guid GameId, Guid GenreId) {
            await dbSet.AddAsync(new GameGenre { GameId = GameId, GenreId = GenreId });
        }
        public async Task<IEnumerable<Guid>> GetGameGuidsByGenreGuidId(Guid genreId) {
            return await dbSet.Where(x => x.GenreId == genreId).Select(x => x.GameId).ToListAsync();
        }
        public async Task<IEnumerable<Guid>> GetGenreGuidsByGameGuidId(Guid gameId) {
            return await dbSet.Where(x => x.GameId == gameId).Select(x => x.GenreId).ToListAsync();
        }

        public async Task Update(Guid gameGuid, List<Guid> genreGuids) {
            foreach (var item in dbSet.Where(x => x.GameId == gameGuid)) {
                context.Remove(item);
            }
            foreach (var item in genreGuids) {
                await dbSet.AddAsync(new GameGenre { GameId = gameGuid, GenreId = item });
            }

        }
    }
}
