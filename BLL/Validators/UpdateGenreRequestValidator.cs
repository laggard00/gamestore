using FluentValidation;
using GameStore.BLL.DTO.Genres;
using GameStore.DAL.Repositories.RepositoryInterfaces;

namespace GameStore.BLL.Validators
{
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
                .WhenAsync(async(x,token) => { return x.genre.ParentGenreId.HasValue; })
                .WithMessage("That parent genre doesn't exist");

        }

    }
}
