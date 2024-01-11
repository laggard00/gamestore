using DAL.Repositories;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore.DAL.Repositories;
using GameStore_DAL.Repositories;
using BLL.AutoMapper;
using BLL.Services;
using GameStore.BLL.Services;

namespace GameStore.WEB.ServiceCollections
{
    public static class BusinessLogicLayerExtensions
    {
        public static IServiceCollection AddBusinessLogicLayerDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<GameService>();
            services.AddScoped<GenreService>();
            services.AddScoped<PlatformService>();
            services.AddScoped<PublisherService>();
            services.AddScoped<OrderCartService>();
            services.AddScoped<CommentService>();
            return services;
        }
    }
}
