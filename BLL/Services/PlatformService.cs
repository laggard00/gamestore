using AutoMapper;
using DAL.Repositories;
using GameStore.BLL.DTO.Platform;
using GameStore.DAL.Repositories.RepositoryInterfaces;
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
        private IMapper mapper { get; set; }

        public PlatformService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork; 
            mapper = _mapper;
        }

        public async Task<IEnumerable<Platform>> GetAllAsync()
        {
            var allPlatforms = await uow.PlatformRepository.GetAllAsync();

            return allPlatforms;
        }

        public async Task<Platform> GetByIdAsync(Guid id)
        {
            var platformById = await uow.PlatformRepository.GetByIdAsync(id);

            return platformById;
        }

        public async Task AddAsync(PlatformDTO model)
        {
            await uow.PlatformRepository.AddAsync(mapper.Map<Platform>(model));

            await uow.SaveAsync();
        }
        public async Task<IEnumerable<Platform>> GetPlatformByGameGuid(Guid gameId)
        {
            var platformGuids = await uow.GamePlatformRepository.GetPlatformGuidsByGameGuidId(gameId);
            var platform = await uow.PlatformRepository.GetAllByPlatformGuids(platformGuids);
            return platform;
        }

        public async Task UpdateAsync(Platform model)
        {
            uow.PlatformRepository.Update(model);
            await uow.SaveAsync();
        }

        public async Task DeleteAsync(Guid modelId)
        {
            await uow.PlatformRepository.DeleteByIdAsync(modelId);
            await uow.SaveAsync();
        }
    }
}
