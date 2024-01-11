using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class AddPlatformRequest
    {
        public PlatformDTO platform { get; set; }
    }
    public class PlatformDTO
    { 
        public string type { get; set; }
    }
    public class AddPlatfromRequestValidator :AbstractValidator<AddPlatformRequest> 
    {
        public AddPlatfromRequestValidator()
        {
            RuleFor(x => x.platform.type)
                .NotEmpty()
                .WithMessage("platform type can't be empty");
        }
    }
}
