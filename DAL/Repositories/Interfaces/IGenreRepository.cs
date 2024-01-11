using GameStore_DAL.Models;

namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface IGenreRepository
    {
        Task AddAsync(GenreEntity entity);
        Task<bool> CheckIfGenreGuidsExist(IEnumerable<Guid> Guids);
        Task<bool> CheckIfGenreGuidExist(Guid Guid);
        void Delete(GenreEntity entity);
        Task DeleteByIdAsync(Guid id);
        Task<IEnumerable<GenreEntity>> GetAllAsync();
        Task<IEnumerable<GenreEntity>> GetAllByGenreGuids(IEnumerable<Guid> GenreGuids);
        Task<IEnumerable<GenreEntity>> GetAllByParentGenreAsync(Guid parentId);
        Task<GenreEntity> GetByIdAsync(Guid id);
        void Update(GenreEntity entity);

        Task<bool> GenreIdExistsAndNotSameAsTheParent(Guid? parentId, Guid id);
    }
}