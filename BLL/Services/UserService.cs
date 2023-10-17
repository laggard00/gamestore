using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService :IUserService
    {
        private IUnitOfWork uow { get; set; }
        private IGamesRepository repository { get; set; }
        private IGamePlatformRepository platform { get; set; }

        private IMapper mapper { get; set; }

        public UserService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;
            mapper = _mapper;
            repository = unitOfWork.GamesRepository;
            platform = unitOfWork.GamePlatformRepository;

        }

        public async Task<IEnumerable<GameDTO>> GetAllAsync()
        {
            var a = await repository.GetAllAsync();
            return a.Select(x => mapper.Map<GameDTO>(x));
        }

        public async Task<GameDTO> GetByIdAsync(int id)
        {
            var getById = await repository.GetByIdAsync(id);
            
            return mapper.Map<GameDTO>(getById);
        }

        public async Task AddAsync(GameDTO model)
        {
            await repository.AddAsync(mapper.Map<GameEntity>(model));
            await uow.SaveAsync();
        }

        public Task UpdateAsync(GameDTO model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int modelId)
        {
            throw new NotImplementedException();
        }

        public async Task<GameDTO> GetGameByAlias(string alias)
        {
            
            var allGames = await repository.GetAllAsync();


            var findByAlias = allGames.SingleOrDefault(x => x.GameAlias.ToLower() == alias.ToLower());

            
            return mapper.Map<GameDTO>(findByAlias); 
           

            
            
        }
    }
}
