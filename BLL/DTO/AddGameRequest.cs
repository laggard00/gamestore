using FluentValidation;
using GameStore.DAL.Repositories.RepositoryInterfaces;
using GameStore_DAL.Data;
using GameStore_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class AddGameRequest
    {
        public GameDTO Game { get; set; }
        public List<Guid> Genres { get; set; }
        public List<Guid> Platforms { get; set; }
        public Guid Publisher { get; set; }
    }

    public class GameDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public double price { get; set; }
        public int unitInStock { get; set; }
        public int discount { get; set; }
    }

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
                .MustAsync(async (key, token) => { return await gamesRepository.IsUnique(key);})
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
                .MustAsync(async (guids, token) => {return await genreRepository.CheckIfGenreGuidsExist(guids); })
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
