using AutoMapper;
using DAL.Repositories;
using GameStore.BLL.DTO;
using GameStore.DAL.Repositories.RepositoryInterfaces;
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
        private IMapper mapper { get; set; }

        public GenreService(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            uow = unitOfWork;
            mapper = _mapper; 

        }
        public async Task<IEnumerable<GetGenreRequest>> GetAllAsync()
        {
            var allGenres = await uow.GenreRepository.GetAllAsync();
            return allGenres.Select(x=> mapper.Map<GetGenreRequest>(x)).ToList();

        }
        public async Task<GenreEntity> GetByIdAsync(Guid id)
        {
            return await uow.GenreRepository.GetByIdAsync(id);

        }
        public async Task AddAsync(GenreDTO model)
        {
            await uow.GenreRepository.AddAsync(mapper.Map<GenreEntity>(model));
            await uow.SaveAsync();
        }

        public async Task UpdateAsync(GenreEntity model)
        {
           uow.GenreRepository.Update(model);
           await uow.SaveAsync();
        }

        public async Task DeleteAsync(Guid modelId)
        {
            await uow.GenreRepository.DeleteByIdAsync(modelId);
            await uow.SaveAsync();
        }

        public async Task<IEnumerable<GetGenreRequest>> GetGenresByGameGuid(Guid gameId) 
        {
            var genreGuids = await uow.GameGenreRepository.GetGenreGuidsByGameGuidId(gameId);
            var genre = await uow.GenreRepository.GetAllByGenreGuids(genreGuids);
            return genre.ToList().Select(x=> mapper.Map<GetGenreRequest>(x));
        }

        public async Task<IEnumerable<GetGenreRequest>> GetGenresByParentGenre(Guid parentId)
        {
            var genres = await uow.GenreRepository.GetAllByParentGenreAsync(parentId);
            return genres.Select(x => mapper.Map<GetGenreRequest>(x));
        }
    }
}
