using AutoMapper;
using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using DAL.Interfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AdminGameService: IAdminGameService
    {
        public IUnitOfWork uow { get; set; }
        public IGamesRepository repository { get; set; }
        public IGamePlatformRepository platform { get; set; }

        public IMapper mapper { get; set; }

        public AdminGameService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;
            mapper = _mapper;
            repository = unitOfWork.GamesRepository;
            platform = unitOfWork.GamePlatformRepository;
            
        }

        
        
        public async Task<GameDTO> GetByIdAsync(int id)
        {

            var getById = await repository.GetByIdAsync(id);
            return mapper.Map<GameDTO>(getById);    

        }

        public async Task<IEnumerable<GameDTO>> GetAllAsync()
        {
            var a = await repository.GetAllAsync();
            return a.Select(x => mapper.Map<GameDTO>(x));
        }

        

        public async Task AddAsync(GameDTO model)
        {
            await repository.AddAsync(mapper.Map<GameEntity>(model));
            await uow.SaveAsync();
        }

        public async Task UpdateAsync(GameDTO model)
        {
            repository.Update(mapper.Map<GameEntity>(model));
            await uow.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await repository.DeleteByIdAsync(modelId);
            await uow.SaveAsync();
        }

        public async Task<IEnumerable<GameDTO>> GetGamesByGenre(int genreId)
        {
            var allGames = await repository.GetAllAsync();
            var filteredByGenre = allGames.Where(x => x.GenreId == genreId).Select(x=> mapper.Map<GameDTO>(x));
            return filteredByGenre;

        }

        public async Task<IEnumerable<GameDTO>> GetGamesByPlatfrom(int platfromId)
        {
            var allGames = await platform.GetGamePlatformByPlatfromId(platfromId);
            var selectingJustGames = allGames.Select(x => x.Game);
            

            return selectingJustGames.Select(x=> mapper.Map<GameDTO>(x));

        }

        public async Task<GameDTO> GetGameByAlias(string alias)
        {
            var allGames = await repository.GetAllAsync();
            var findByAlias = allGames.SingleOrDefault(x => x.GameAlias.ToLower() == alias.ToLower());
            return mapper.Map<GameDTO>(findByAlias);
        }
    }
}
