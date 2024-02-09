
using DAL.Repositories;
using GameStore.DAL.Repositories;
using GameStore_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface IUnitOfWork
    {
        IGamesRepository GamesRepository { get; }
        IGenreRepository GenreRepository { get; }
        IPlatformRepository PlatformRepository { get; }

        IGamePlatformRepository GamePlatformRepository { get; }
        IGameGenreRepository GameGenreRepository { get; }
        IPublisherRepository PublisherRepository { get; }
        IOrderCartRepository OrderCartRepository { get; }
        ICommentRepository CommentRepository { get; }

        Task SaveAsync();
    }
}
