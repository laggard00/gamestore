namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface IGameGenreRepository
    {
        Task AddGameGenre(Guid GameId, Guid GenreId);
        Task<IEnumerable<Guid>> GetGameGuidsByGenreGuidId(Guid genreId);
        Task<IEnumerable<Guid>> GetGenreGuidsByGameGuidId(Guid gameId);
        Task Update(Guid gameGuid, List<Guid> genreGuids);
    }
}