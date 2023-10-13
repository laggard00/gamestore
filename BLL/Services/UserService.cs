using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService :IUserService
    {
        public IUnitOfWork uow { get; set; }
        public IGamesRepository repository { get; set; }
        public IGamePlatformRepository platform { get; set; }

        public IMapper mapper { get; set; }

        public UserService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;
            mapper = _mapper;
            repository = unitOfWork.GamesRepository;
            platform = unitOfWork.GamePlatformRepository;

        }

        public Task<IEnumerable<GameDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
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
    }
}
