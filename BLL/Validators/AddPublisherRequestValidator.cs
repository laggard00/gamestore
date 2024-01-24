using FluentValidation;
using GameStore.BLL.DTO.Publisher;

namespace GameStore.BLL.Validators
{
    public class AddPublisherRequestValidator : AbstractValidator<AddPublisherRequest>
    {
        public AddPublisherRequestValidator()
        {
            RuleFor(x => x.publisher.companyName)
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage("Company name is mandatory and should be larger than 2");
            RuleFor(x => x.publisher.homePage)
                .Must((x) => Uri.IsWellFormedUriString(x, UriKind.Absolute))
                .WithMessage("Home page must be correct link");
            RuleFor(x => x.publisher.description)
                .NotEmpty()
                .WithMessage("Description can't be empty");
        }

    }
}
