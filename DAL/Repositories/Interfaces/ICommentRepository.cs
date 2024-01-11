using GameStore.DAL.Models;
using System.Linq.Expressions;

namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface ICommentRepository
    {
        Task AddAsync(Comment comment);
        void DeleteComment(Guid id, Guid commentId);
        Task<IEnumerable<Comment>> GetAllByGameId(Guid key);
        Task<Comment> GetById(Guid? Id);
        Task<bool> ParentExist(Guid? parentGuid);
    }
}