using FluentValidation;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class AddGenreRequest
    {
        public GenreDTO genre { get; set; }
    }
    public class GenreDTO { 
        public string name { get; set; }
        public Guid parentGenreId { get; set; }
    }

    public class AddGenreRequestValidator : AbstractValidator<AddGenreRequest> 
    {
        private readonly IGenreRepository genreRepository;
        public AddGenreRequestValidator(IGenreRepository genre)
        {
            genreRepository = genre;
            RuleFor(x => x.genre.name).NotEmpty().WithMessage("Genre name can't be empty");
            RuleFor(x => x.genre.parentGenreId)
                .MustAsync(async (guid, token) => { return await genreRepository.CheckIfGenreGuidExist(guid); })
                .WithMessage("Parent genre must exists");
            
        }
    }
}
