namespace GameStore.DAL.Repositories.RepositoryInterfaces
{
    public interface IGamePlatformRepository
    {
        Task AddGamePlatform(Guid GameId, Guid PlatformId);
        Task<IEnumerable<Guid>> GetGameGuidsByPlatformId(Guid platformId);
        Task<IEnumerable<Guid>> GetPlatformGuidsByGameGuidId(Guid gameId);
        Task Update(Guid gameGuid, List<Guid> platformGuids);
    }
}