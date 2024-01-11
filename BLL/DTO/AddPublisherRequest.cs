using FluentValidation;
using GameStore_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class AddPublisherRequest
    {
        public PublisherDTO publisher { get; set; }
    }
    public class PublisherDTO
    { 
        public string companyName { get; set; }
        public string homePage { get; set; }
        public string description { get; set; }
    }

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
