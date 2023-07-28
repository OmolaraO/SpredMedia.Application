using FluentValidation;
using SpredMedia.Authentication.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class KeyAndIvValidator  : AbstractValidator<KeyAndIv>
    {
        public KeyAndIvValidator()
        {
            RuleFor(Dto => Dto.ValueForKey)
                .NotNull().NotEmpty().WithMessage("THe key must not be empty");
            RuleFor(Dto => Dto.ValueForIV)
                .NotNull().WithMessage("IV must not be null")
                .NotEmpty().WithMessage("IV must not be empty string");
        }
    }
}
