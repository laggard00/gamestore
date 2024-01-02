using GameStore.DAL.Filters;
using GameStore_DAL.Models;

namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface IGamesRepository
    {
        Task<Game> AddAsync(Game entity);
        Task<IQueryable<Game>> BuildQuery(GameFilter filter);
        void Delete(Game entity);
        Task DeleteByKeyAsync(string key);
        Task<IEnumerable<Game>> GetAllAsync(GameFilter filters);
        Task<IEnumerable<Game>> GetAllByGameGuids(IEnumerable<Guid> GameGuids);
        Task<IEnumerable<Game>> GetAllGamesWithSamePublisher(Guid id);
        Task<Game> GetByIdAsync(Guid id);
        Task<Game> GetGameByAlias(string alias);
        Task Update(Game entity);
    }
}