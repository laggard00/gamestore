using FluentValidation;
using GameStore.BLL.DTO.Platform;
using GameStore.DAL.Repositories.RepositoryInterfaces;

namespace GameStore.BLL.Validators
{
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
