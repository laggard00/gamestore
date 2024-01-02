using GameStore_DAL.Models;

namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface IPlatformRepository
    {
        Task AddAsync(Platform entity);
        Task<bool> CheckIfPlatformGuidsExist(IEnumerable<Guid> Guids);
        void Delete(Platform entity);
        Task DeleteByIdAsync(Guid id);
        Task<IEnumerable<Platform>> GetAllAsync();
        Task<IEnumerable<Platform>> GetAllByPlatformGuids(IEnumerable<Guid> platformGuids);
        Task<Platform> GetByIdAsync(Guid id);
        void Update(Platform entity);
    }
}