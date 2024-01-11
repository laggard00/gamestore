using DAL.Repositories;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore.DAL.Repositories;
using GameStore_DAL.Repositories;

namespace GameStore.WEB.ServiceCollections
{
    public static class DataAccessLayerExtensions
    {
       public static IServiceCollection AddDataAccessLayerDependencies(this IServiceCollection services)

        {
            services.AddScoped<IGamesRepository, GamesRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IGameGenreRepository, GameGenreRepository>();
            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddScoped<IGamePlatformRepository, GamePlatformRepository>();
            services.AddScoped<IPublisherRepository, PublisherRepository>();
            services.AddScoped<IOrderCartRepository, OrderCartRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;

        }

    }
}
