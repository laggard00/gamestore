using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq.Expressions;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using DAL.Models;
using GameStore_DAL.Data;
using GameStore.BLL.DTO;
using GameStore.DAL.Models;

namespace BLL.AutoMapper
{
    public class AutoMapperProfile : Profile, IMapper
    {
        
        public AutoMapperProfile() {



            CreateMap<AddGameRequest, Game>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Game.Name))
                    .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Game.Key))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Game.Description))
                    .ForMember(dest => dest.UnitInStock, opt => opt.MapFrom(src => src.Game.unitInStock))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Game.price))
                    .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Game.discount))
                    .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.Publisher));

            CreateMap<GenreDTO, GenreEntity>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                    .ForMember(dest => dest.ParentGenreId, opt => opt.MapFrom(src => src.parentGenreId));

            CreateMap<PlatformDTO, Platform>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.type));

            CreateMap<PublisherDTO, Publisher>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.HomePage, opt => opt.MapFrom(src => src.homePage))
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.companyName))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description));

            CreateMap<GenreEntity, GetGenreRequest>()
                    .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Order,OrderDTO>()
                    .ForMember(dest=> dest.date, opt=> opt.MapFrom(src=>src.Date))
                    .ForMember(dest=> dest.id,opt=>opt.MapFrom(src=>src.Id))
                    .ForMember(dest=>dest.customerId, opt=>opt.MapFrom(src=> src.CustomerId));

            CreateMap<OrderGame, OrderGameDTO>()
                    .ForMember(dest => dest.productId, opt => opt.MapFrom(src => src.ProductId))
                    .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
                    .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
                    .ForMember(dest => dest.discount, opt => opt.MapFrom(src => src.Discount));

            CreateMap<OrderGameDTO, OrderGame>()
                    .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.productId))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price))
                    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quantity))
                    .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.discount));

            CreateMap<Comment, GetCommentRequest>()
                    .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.body, opt => opt.MapFrom(src => src.Body))
                    .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.childComments, opt => opt.MapFrom(src => src.Children));

                    

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
