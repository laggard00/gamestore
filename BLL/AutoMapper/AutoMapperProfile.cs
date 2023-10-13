﻿using Microsoft.AspNetCore.Mvc.Infrastructure;
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
                     .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))

                     .ForMember(dest => dest.GamePlatforms, opt => opt.MapFrom(src => src.PlatformId.Select(platformId => new GamePlatform { PlatformId = platformId }))) ;


            CreateMap<GameEntity, GameDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.GameAlias, opt => opt.MapFrom(src => src.GameAlias))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
                    .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.GamePlatforms.Select(x => x.PlatformId)));

            CreateMap<Genre, GenreDTO>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GenreName));

            CreateMap<GenreDTO, Genre>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Platform, PlatformDTO>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PlatformName));

            CreateMap<PlatformDTO, Platform>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.Name));

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
