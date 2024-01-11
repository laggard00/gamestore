using DAL.Repositories;
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
    public class UpdatePlatformRequest
    {
        public GameStore_DAL.Models.Platform platform { get; set; }
    }

    public class UpdatePlatformRequestValidator : AbstractValidator<UpdatePlatformRequest>
    {
        private readonly IPlatformRepository platformRepository;
        public UpdatePlatformRequestValidator(IPlatformRepository platform)
        {
            platformRepository = platform;

        RuleFor(x => x.platform.Id)
                .MustAsync(async (id, token) => { return await platformRepository.GetByIdAsync(id) != null; })
                .WithMessage("Platform Id must by associated with real platform");

            RuleFor(x => x.platform.Type)
                .NotEmpty()
                .WithMessage("platform type can't be empty");
        }

        
    }
}
