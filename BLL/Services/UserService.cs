﻿using AutoMapper;
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

        public async Task<GameDTO> GetGameByAlias(string alias)
        {
            

            var findByAlias = await repository.GetGameByAlias(alias);

            
            return mapper.Map<GameDTO>(findByAlias); 
           

            
            
        }
    }
}
