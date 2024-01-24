using FluentValidation;
using GameStore.BLL.DTO.Platform;

namespace GameStore.BLL.Validators
{
    public class AddPlatfromRequestValidator : AbstractValidator<AddPlatformRequest>
    {
        public AddPlatfromRequestValidator()
        {
            RuleFor(x => x.platform.type)
                .NotEmpty()
                .WithMessage("platform type can't be empty");
        }
    }
}
