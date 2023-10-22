﻿using AutoMapper;
using BLL.DTO;
using BLL.Interfaces.IAdminINTERFACES;
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
    public class AdminGenreService : IAdminGenreService
    {
        private IUnitOfWork uow { get; set; }

        private IGenreRepository repository { get; set; }

        private IMapper mapper { get; set; }

        public AdminGenreService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;

            mapper = _mapper;

            repository = unitOfWork.GenreRepository;
            

        }



        public async Task<IEnumerable<GenreDTO>> GetAllAsync()
        {
            var get = await repository.GetAllAsync();

            return get.Select(x=> mapper.Map<GenreDTO>(x)).ToList();

        }




        public async Task<GenreDTO> GetByIdAsync(int id)
        {
            var get = await repository.GetByIdAsync(id);

            return mapper.Map<GenreDTO>(get);

        }



        public async Task AddAsync(GenreDTO model)
        {
            await repository.AddAsync(mapper.Map<GenreEntity>(model));

            await uow.SaveAsync();
        }

        public async Task UpdateAsync(GenreDTO model)
        {

           repository.Update(mapper.Map<GenreEntity>(model));

           await uow.SaveAsync();

        }

        public async Task DeleteAsync(int modelId)
        {

            await repository.DeleteByIdAsync(modelId);


            await uow.SaveAsync();

        }
    }
}
