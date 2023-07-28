using FluentValidation;
using SpredMedia.Authentication.Core.DTO;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class ResendOtpValidator : AbstractValidator<ResendOtpDTO>
    {
        public ResendOtpValidator()
        {
            RuleFor(ResendOtpDTO => ResendOtpDTO.Email)
                .EmailAddress().WithMessage("Is not a valid email");
            RuleFor(ResendOtpDTO => ResendOtpDTO.Purpose)
                .NotNull().WithMessage("message must not be null")
                .NotEmpty().WithMessage("message must not be empty string");
        }
    }
}
