
using DAL.Repositories;
using GameStore.DAL.Repositories;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore_DAL.Interfaces
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
