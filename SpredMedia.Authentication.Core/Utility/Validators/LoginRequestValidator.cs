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
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(LoginRequestDto => LoginRequestDto.Password)
                .Password();
            RuleFor(LoginRequestDto => LoginRequestDto.Email)
                .Address();
        }
    }
}
