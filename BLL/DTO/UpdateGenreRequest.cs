using FluentValidation;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class UpdateGenreRequest
    {
        public GenreEntity genre { get; set; }
    }
    public class UpdateGenreRequestValidator : AbstractValidator<UpdateGenreRequest>
    {
        private readonly IGenreRepository genreRepository;
        public UpdateGenreRequestValidator(IGenreRepository genre)
        {
            genre = genreRepository;

            RuleFor(x => x.genre.Id)
                .MustAsync(async (model, id, token) => { return await genreRepository.GenreIdExistsAndNotSameAsTheParent(model.genre.ParentGenreId, id); })
                .WithMessage("Genre with that id doesn't exist");

            RuleFor(x => x.genre.Name)
                .NotEmpty()
                .WithMessage("Genre name can't be empty");

            RuleFor(x => x.genre.ParentGenreId)
                .MustAsync(async (id, token) => { return await genreRepository.CheckIfGenreGuidExist(id.Value); })
                .When(x => x.genre.ParentGenreId.HasValue)
                .WithMessage("That parent genre doesn't exist");

        }

    }
}
