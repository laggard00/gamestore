using FluentValidation;
using GameStore.BLL.DTO.Publisher;
using GameStore.BLL.DTO.PublishersDTO;
using GameStore.DAL.Repositories.RepositoryInterfaces;

namespace GameStore.BLL.Validators
{
    public class UpdatePublisherRequestValidator : AbstractValidator<UpdatePublisherRequest>
    {
        private readonly IPublisherRepository publisherRepository;
        public UpdatePublisherRequestValidator(IPublisherRepository publisher)
        {
            publisherRepository = publisher;

            RuleFor(x => x.publisher.Id)
                .Must(publisherRepository.CheckIfPublisherExists)
                .WithMessage("Publisher Id is not associated with any publisher in database");

            RuleFor(x => x.publisher.CompanyName)
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage("Company name is mandatory and should be larger than 2");

            RuleFor(x => x.publisher.HomePage)
                .Must((x) => Uri.IsWellFormedUriString(x, UriKind.RelativeOrAbsolute))
                .WithMessage("Home page must be correct link");

            RuleFor(x => x.publisher.Description)
                .NotEmpty()
                .WithMessage("Description can't be empty");

        }


    }
}
