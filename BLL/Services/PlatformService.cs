using AutoMapper;
using DAL.Repositories;
using GameStore.BLL.DTO;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PlatformService
    {
        private IUnitOfWork uow { get; set; }

        private PlatformRepository platformRepository { get; set; }
        private IMapper mapper { get; set; }

        public PlatformService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;
            platformRepository = unitOfWork.PlatformRepository;
            mapper = _mapper;
        }

        public async Task<IEnumerable<GameStore_DAL.Models.Platform>> GetAllAsync()
        {
            var allPlatforms = await platformRepository.GetAllAsync();

            return allPlatforms;
        }

        public async Task<GameStore_DAL.Models.Platform> GetByIdAsync(Guid id)
        {
            var platformById = await platformRepository.GetByIdAsync(id);

            return platformById;
        }

        public async Task AddAsync(GameStore.BLL.DTO.PlatformDTO model)
        {
            await platformRepository.AddAsync(mapper.Map<GameStore_DAL.Models.Platform>(model));

            await uow.SaveAsync();
        }
        public async Task<IEnumerable<GameStore_DAL.Models.Platform>> GetPlatformByGameGuid(Guid gameId)
        {
            var platformGuids = await uow.GamePlatformRepository.GetPlatformGuidsByGameGuidId(gameId);
            var platform = await uow.PlatformRepository.GetAllByPlatformGuids(platformGuids);
            return platform;
        }

        public async Task UpdateAsync(GameStore_DAL.Models.Platform model)
        {
            platformRepository.Update(model);
            await uow.SaveAsync();
        }

        public async Task DeleteAsync(Guid modelId)
        {
            platformRepository.DeleteByIdAsync(modelId);

            await uow.SaveAsync();
        }
    }
}
