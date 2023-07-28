

using FluentValidation;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class GoogleLoginRequestValidator : AbstractValidator<GoogleLoginRequestDTO>
    {
        public GoogleLoginRequestValidator()
        {
            RuleFor(GoogleLoginRequestDTO => GoogleLoginRequestDTO.Name)
                .HumanName();
            RuleFor(GoogleLoginRequestDTO => GoogleLoginRequestDTO.Email)
                .Address();
        }
    }
}
