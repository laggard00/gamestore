
using DAL.Repositories;
using GameStore.DAL.Repositories;
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
        GamesRepository GamesRepository { get; }
        GenreRepository GenreRepository { get; }
        PlatformRepository PlatformRepository { get; }

        GamePlatformRepository GamePlatformRepository { get; }
        GameGenreRepository GameGenreRepository { get; }
        PublisherRepository PublisherRepository { get; }
        OrderCartRepository OrderCartRepository { get; }
        CommentRepository CommentRepository { get; }

        Task SaveAsync();
    }
}
