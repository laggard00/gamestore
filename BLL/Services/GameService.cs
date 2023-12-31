using AutoMapper;
using DAL.Exceptions;
using DAL.Repositories;
using GameStore.BLL.DTO;
using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
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
        private GamesRepository repository { get; set; }
        private GamePlatformRepository platform { get; set; }

        private IMapper mapper { get; set; }

        public GameService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;
            mapper = _mapper;
            repository = unitOfWork.GamesRepository;
            platform = unitOfWork.GamePlatformRepository;
            
        }
        public async Task<Game> GetByIdAsync(Guid id)
        {
            var getById = await repository.GetByIdAsync(id);
            return getById; 
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await repository.GetAllAsync();
            
        }

        public async Task AddAsync(POST_GameDTO model)
        {
             var gameId = await uow.GamesRepository.AddAsync(mapper.Map<Game>(model));
             foreach (var item in model.Platform)
             {
                 await uow.GamePlatformRepository.AddGamePlatform(gameId.Id, item);
             }
             foreach (var item in model.Genres)
             {
                 await uow.GameGenreRepository.AddGameGenre(gameId.Id, item);
             }
             await uow.SaveAsync();   
        }

        public async Task UpdateAsync(PUT_GameDTO model)
        {
            if (!await uow.PlatformRepository.CheckIfPlatformGuidsExist(model.Platform) || !await uow.GenreRepository.CheckIfGenreGuidsExist(model.Genre))
            {
                throw new Exception("Genres or platform currently don't exist");
            }
            await uow.GamesRepository.Update(model.Game);
            await uow.GameGenreRepository.Update(model.Game.Id, model.Genre);
            await uow.GamePlatformRepository.Update(model.Game.Id, model.Platform);
            await uow.SaveAsync();
        }

        public async Task DeleteAsync(string key)
        {
            await repository.DeleteByKeyAsync(key);
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
            var allGamesGuid = await platform.GetGameGuidsByPlatformId(platformId);
            var Games = await repository.GetAllByGameGuids(allGamesGuid);
            return Games;

        }

        public async Task<Game> GetGameByAlias(string alias)
        {
            var findByAlias = await repository.GetGameByAlias(alias);
            return findByAlias;
        }
    }
}
