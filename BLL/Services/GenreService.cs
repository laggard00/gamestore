﻿using AutoMapper;
using DAL.Repositories;
using GameStore.BLL.DTO;
using GameStore_DAL.Interfaces;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GenreService
    {
        private IUnitOfWork uow { get; set; }

        private GenreRepository repository { get; set; }

        private IMapper mapper { get; set; }

        public GenreService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;

            mapper = _mapper;

            repository = unitOfWork.GenreRepository;


        }
        public async Task<IEnumerable<GET_Genre>> GetAllAsync()
        {
            var allGenres = await repository.GetAllAsync();
            return allGenres.Select(x=> mapper.Map<GET_Genre>(x)).ToList();

        }
        public async Task<GenreEntity> GetByIdAsync(Guid id)
        {
            return await repository.GetByIdAsync(id);

            

        }
        public async Task AddAsync(GenreDTO model)
        {
            await repository.AddAsync(mapper.Map<GenreEntity>(model));

            await uow.SaveAsync();
        }

        public async Task UpdateAsync(GenreEntity model)
        {
           repository.Update(model);

           await uow.SaveAsync();
        }

        public async Task DeleteAsync(Guid modelId)
        {
            await repository.DeleteByIdAsync(modelId);
            await uow.SaveAsync();
        }

        public async Task<IEnumerable<GET_Genre>> GetGenresByGameGuid(Guid gameId) 
        {
            var genreGuids = await uow.GameGenreRepository.GetGenreGuidsByGameGuidId(gameId);
            var genre = await uow.GenreRepository.GetAllByGenreGuids(genreGuids);
            return genre.ToList().Select(x=> mapper.Map<GET_Genre>(x));
        }

        public async Task<IEnumerable<GET_Genre>> GetGenresByParentGenre(Guid parentId)
        {
            var genres = await uow.GenreRepository.GetAllByParentGenreAsync(parentId);
            return genres.Select(x => mapper.Map<GET_Genre>(x));
        }
    }
}