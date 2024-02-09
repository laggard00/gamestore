using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore_DAL.Repositories;
using DAL.Repositories;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;

namespace GameStore.DAL.Repositories {
    public class UnitOfWork : IUnitOfWork {
        private readonly GameStoreDbContext context;
        public IGamesRepository GamesRepository { get; }
        public IGenreRepository GenreRepository { get; }
        public IPlatformRepository PlatformRepository { get; }
        public IGamePlatformRepository GamePlatformRepository { get; }
        public IGameGenreRepository GameGenreRepository { get; }
        public IPublisherRepository PublisherRepository { get; }
        public IOrderCartRepository OrderCartRepository { get; }
        public ICommentRepository CommentRepository { get; }

        public UnitOfWork(GameStoreDbContext context, IGamesRepository gamesRepository,
                          IGenreRepository genreRepository, IPlatformRepository platformRepository,
                          IGamePlatformRepository gamePlatformRepository, IPublisherRepository publisherRepository, IGameGenreRepository gameGenre, IOrderCartRepository ocrepository, ICommentRepository cmRepository) {
            this.context = context;
            GamesRepository = gamesRepository;
            GenreRepository = genreRepository;
            PlatformRepository = platformRepository;
            GamePlatformRepository = gamePlatformRepository;
            PublisherRepository = publisherRepository;
            GameGenreRepository = gameGenre;
            OrderCartRepository = ocrepository;
            CommentRepository = cmRepository;
        }

        public async Task SaveAsync() {
            await context.SaveChangesAsync();
        }
    }
}
