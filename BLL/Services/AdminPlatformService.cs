using AutoMapper;
using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
using DAL.Interfaces;
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
    public class AdminPlatformService :IAdminPlatformService
    {
        public IUnitOfWork uow { get; set; }

        public IPlatformRepository platformRepository { get; set; }
        public IMapper mapper { get; set; }

        public AdminPlatformService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;
            platformRepository = unitOfWork.PlatformRepository;
            mapper = _mapper;
        }

        public async Task<IEnumerable<PlatformDTO>> GetAllAsync()
        {
            var get = await platformRepository.GetAllAsync();

            return get.Select(x => mapper.Map<PlatformDTO>(x));
        }

        public async Task<PlatformDTO> GetByIdAsync(int id)
        {
            var get = await platformRepository.GetByIdAsync(id);

            return mapper.Map<PlatformDTO>(get);
        }

        public async Task AddAsync(PlatformDTO model)
        {
            await platformRepository.AddAsync(mapper.Map<Platform>(model));

            await uow.SaveAsync();
        }

        public async Task UpdateAsync(PlatformDTO model)
        {
            platformRepository.Update(mapper.Map<Platform>(model));

            await uow.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            platformRepository.DeleteByIdAsync(modelId);

            await uow.SaveAsync();
        }
    }
}
