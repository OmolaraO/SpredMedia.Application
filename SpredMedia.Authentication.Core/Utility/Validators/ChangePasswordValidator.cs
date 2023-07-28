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
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordValidator()
        {
            RuleFor(ChangePasswordDTO => ChangePasswordDTO.CurrentPassword)
                .Password();
            RuleFor(ChangePasswordDTO => ChangePasswordDTO.NewPassword) 
                .Password();
            RuleFor(ChangePasswordDTO => ChangePasswordDTO.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
