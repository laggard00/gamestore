using FluentValidation;
using GameStore.BLL.DTO.Games;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using System.ComponentModel.DataAnnotations;

namespace GameStore.BLL.Validators
{
    public class AddGameRequestValidator : AbstractValidator<AddGameRequest>
    {

        private readonly IGenreRepository genreRepository;
        private readonly IPlatformRepository platformRepository;
        private readonly IPublisherRepository publisherRepository;
        private readonly IGamesRepository gamesRepository;
        public AddGameRequestValidator(IGenreRepository genre, IPlatformRepository platform, IPublisherRepository publisher, IGamesRepository games)
        {

            genreRepository = genre;
            platformRepository = platform;
            publisherRepository = publisher;
            gamesRepository = games;

            RuleFor(x => x.Game.Name)
                .NotEmpty()
                .WithMessage("Name of the game is required");

            RuleFor(x => x.Game.Key)
                .MustAsync(async (key, token) => { return await gamesRepository.IsUnique(key); })
                .WithMessage("Game Key must be a unique key");

            RuleFor(x => x.Game.Description)
                .NotEmpty()
                .WithMessage("Please write some description");

            RuleFor(x => x.Game.price)
                .Must((x) => x > 0)
                .NotEmpty()
                .WithMessage("Price must be larger than zero ( hint add discount as 100% if you want to actually set price to 0)");

            RuleFor(x => x.Game.unitInStock)
                .Must((x) => x > 0)
                .NotEmpty()
                .WithMessage("Unit in stock has to be larger than zero");

            RuleFor(x => x.Game.discount)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .NotEmpty().WithMessage("Discount should be between 0 and a 100");

            RuleFor(x => x.Genres).NotEmpty()
                .MustAsync(async (guids, token) => { return await genreRepository.CheckIfGenreGuidsExist(guids); })
                .WithMessage("Not all genres exist in the database");

            RuleFor(x => x.Platforms).NotEmpty()
                .MustAsync(async (guid, token) => { return await platformRepository.CheckIfPlatformGuidsExist(guid); })
                .WithMessage("Not all platforms exist in the database");

            RuleFor(x => x.Publisher)
                .NotEmpty()
                .Must(publisherRepository.CheckIfPublisherExists)
                .WithMessage("Publisher with that id doesn't exist.");

        }

        


    }

}
