using GameStore.DAL.Filters;
using GameStore.DAL.Models;
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
        bool UnitInStockIsLargerThanOrder(IEnumerable<OrderGame> cart);
        Task Update(Game entity);
        Task UpdateUnitInStockFromCart(Guid productId, int quantity);
        Task<bool> IsUnique(string key);
       
        Task<int> GetGameCount(GameFilter filter);
    }
}