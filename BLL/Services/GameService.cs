using AutoMapper;
using DAL.Exceptions;
using DAL.Repositories;
using GameStore.BLL.DTO;
using GameStore.DAL.Filters;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using GameStore_DAL.Repositories;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GameService
    {
        private IUnitOfWork uow { get; set; }
        private IMapper mapper { get; set; }

        public GameService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;
            mapper = _mapper;
            
        }
        public async Task<Game> GetByIdAsync(Guid id)
        {
            var getById = await uow.GamesRepository.GetByIdAsync(id);
            return getById; 
        }

        public async Task<IEnumerable<Game>> GetAllAsync(GameFilter filters)
        {
            return await uow.GamesRepository.GetAllAsync(filters);
            
        }

        public async Task AddAsync(AddGameRequest model)
        {

             var gameId = await uow.GamesRepository.AddAsync(mapper.Map<Game>(model));
             foreach (var item in model.Platforms)
             {
                 await uow.GamePlatformRepository.AddGamePlatform(gameId.Id, item);
             }
             foreach (var item in model.Genres)
             {
                 await uow.GameGenreRepository.AddGameGenre(gameId.Id, item);
             }
             await uow.SaveAsync();   
        }

        public async Task UpdateAsync(UpdateGameRequest model)
        {
            //this part is useless if database is setup correctly.
            if (!await uow.PlatformRepository.CheckIfPlatformGuidsExist(model.Platforms) || !await uow.GenreRepository.CheckIfGenreGuidsExist(model.Genres) || await uow.PublisherRepository.GetPublisherById(model.Publisher) is null)
            {
                throw new Exception("Genres, platform, or publishers currently don't exist");
            }
            model.Game.PublisherId = model.Publisher;
            await uow.GamesRepository.Update(model.Game);
            await uow.GameGenreRepository.Update(model.Game.Id, model.Genres);
            await uow.GamePlatformRepository.Update(model.Game.Id, model.Platforms);
            
            await uow.SaveAsync();
        }

        public async Task DeleteAsync(string key)
        {
            await uow.GamesRepository.DeleteByKeyAsync(key);
            await uow.SaveAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByGenre(Guid genreGuid)
        {
            var allGameGuids = await uow.GameGenreRepository.GetGameGuidsByGenreGuidId(genreGuid);
            var allGames = await uow.GamesRepository.GetAllByGameGuids(allGameGuids);
            return allGames;
        }

        public async Task<IEnumerable<Game>> GetGamesByPlatform(Guid platformId)
        {
            var allGamesGuid = await uow.GamePlatformRepository.GetGameGuidsByPlatformId(platformId);
            var Games = await uow.GamesRepository.GetAllByGameGuids(allGamesGuid);
            return Games;

        }

        public async Task<Game> GetGameByAlias(string alias)
        {
            var findByAlias = await uow.GamesRepository.GetGameByAlias(alias);
            return findByAlias;
        }
    }
}
