using FluentValidation;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordValidator()
        {
            RuleFor(ResetPasswordDTO => ResetPasswordDTO.Token)
                .NotEmpty().WithMessage("THE token must not be empty")
                .NotNull().WithMessage("the token must not be a null value");
            RuleFor(ResetPasswordDTO => ResetPasswordDTO.Email)
                .EmailAddress();
            RuleFor(ResetPasswordDTO => ResetPasswordDTO.NewPassword)
                .Password();
            RuleFor(ResetPasswordDTO => ResetPasswordDTO.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
