using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq.Expressions;
using BLL.DTO;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using DAL.Models;
using GameStore_DAL.Data;

namespace BLL.AutoMapper
{
    public class AutoMapperProfile : Profile, IMapper
    {
        
        public AutoMapperProfile() {



            CreateMap<GameDTO, GameEntity>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.GameAlias, opt => opt.MapFrom(src => src.GameAlias))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                   // .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
                    //.ForMember(dest => dest.GameGenres, opt => opt.MapFrom(src => src.GenreId.Select(genreId => new GameGenre { GenreId = genreId, GameId = src.Id })))
                    //.ForMember(dest => dest.GamePlatforms, opt => opt.MapFrom(src => src.PlatformId.Select(platformId => new GamePlatform { PlatformId = platformId, GameId = src.Id })))
                    .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                    .ForMember(dest => dest.UnitInStock, opt => opt.MapFrom(src => src.UnitInStock))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                    .ForMember(dest => dest.GameGenres, opt => opt.MapFrom(src => src.Genres.Select(x => new GameGenre { GenreId = x.Id, GameId = src.Id })))
                    .ForMember(dest => dest.GamePlatforms, opt => opt.MapFrom(src => src.Platforms.Select(x => new GamePlatform { PlatformId = x.Id, GameId = src.Id })));
                   



            CreateMap<GameEntity, GameDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.GameAlias, opt => opt.MapFrom(src => src.GameAlias))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    //.ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GameGenres.Select(x => x.GenreId)))
                    //.ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.GamePlatforms.Select(x => x.PlatformId)))
                    //
                   // .ForMember(dest=>dest.Publisher, opt=> opt.MapFrom(src=> src.Publisher))
                    .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                    .ForMember(dest => dest.UnitInStock, opt => opt.MapFrom(src => src.UnitInStock))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                    .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.GameGenres.Select(x => x.Genre)))
                    .ForMember(dest => dest.Platforms, opt => opt.MapFrom(src => src.GamePlatforms.Select(x => x.Platform)));
            // .ForMember(dest => dest.GenreNames, opt => opt.MapFrom(src=> src.GameGenres.Select(x=> x.Genre.GenreName))).
            // ForMember(dest => dest.PlatformNames, opt => opt.MapFrom(src=> src.GamePlatforms.Select(x=> x.Platform.Type.ToString())));


            CreateMap<PublisherEntity, PublisherDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(x => x.CompanyName))
                    .ForMember(dest => dest.HomePage, opt => opt.MapFrom(x => x.HomePage))
                    .ForMember(dest => dest.Descritpion, opt => opt.MapFrom(x => x.Descritpion));
                    // .ForMember(dest => dest.Games, opt => opt.MapFrom(x=> x.Games));

            CreateMap<PublisherDTO, PublisherEntity>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(x => x.CompanyName))
                    .ForMember(dest => dest.HomePage, opt => opt.MapFrom(x => x.HomePage))
                    .ForMember(dest => dest.Descritpion, opt => opt.MapFrom(x => x.Descritpion));
                   // .ForMember(dest => dest.Games, opt => opt.MapFrom(x => x.Games));



            CreateMap<GenreEntity, GenreDTO>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GenreName))
                   .ForMember(dest => dest.ParentGenreId, opt => opt.MapFrom(src=> src.ParentGenreId));



            CreateMap<GenreDTO, GenreEntity>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Name))
                   .ForMember(dest=> dest.ParentGenreId, opt=> opt.MapFrom(src=>src.ParentGenreId))
                   .ForMember(dest => dest.SubGenre, opt => opt.Ignore())
                   .ForMember(dest => dest.GameGenres, opt => opt.Ignore());
            
                   

            CreateMap<PlatformEntity, PlatformDTO>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<PlatformDTO, PlatformEntity>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (PlatformType)src.Id));

        }
       

        public IConfigurationProvider ConfigurationProvider => throw new NotImplementedException();

        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions<object, TDestination>> opts)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TDestination>(object source)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand)
        {
            throw new NotImplementedException();
        }

        public IQueryable ProjectTo(IQueryable source, Type destinationType, IDictionary<string, object> parameters = null, params string[] membersToExpand)
        {
            throw new NotImplementedException();
        }
    }
}
