using GameStore_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore_DAL.Repositories;
using DAL.Repositories;
using GameStore.DAL.Repositories;

namespace GameStore_DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameStoreDbContext context;
        public GamesRepository GamesRepository { get; }

        public GenreRepository GenreRepository{ get; }
        public PlatformRepository PlatformRepository{ get; }

        public GamePlatformRepository GamePlatformRepository{ get; }
        public GameGenreRepository GameGenreRepository { get; }
        public PublisherRepository PublisherRepository{ get; }
        public OrderCartRepository OrderCartRepository{ get; }
        public CommentRepository CommentRepository{ get; }


        public UnitOfWork(GameStoreDbContext context, GamesRepository gamesRepository,
                          GenreRepository genreRepository, PlatformRepository platformRepository,
                          GamePlatformRepository gamePlatformRepository, PublisherRepository publisherRepository,GameGenreRepository gameGenre,OrderCartRepository ocrepository,CommentRepository cmRepository)
        {
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
        

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
