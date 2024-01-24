using FluentValidation;
using GameStore.BLL.DTO.Genres;
using GameStore.DAL.Repositories.RepositoryInterfaces;

namespace GameStore.BLL.Validators
{
    public class AddGenreRequestValidator : AbstractValidator<AddGenreRequest>
    {
        private readonly IGenreRepository genreRepository;
        public AddGenreRequestValidator(IGenreRepository genre)
        {
            
            genreRepository = genre;
            RuleFor(x => x.genre.name).MinimumLength(5).WithMessage("Genre name can't be empty");
            RuleFor(x => x.genre.parentGenreId)
                .MustAsync(async (guid, token) => {
                    if (guid.HasValue) { return await genreRepository.CheckIfGenreGuidExist(guid.Value); }
                    else return true; })
                .WithMessage("Parent genre must exists");

        }
    }
}
