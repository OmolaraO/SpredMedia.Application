

using FluentValidation;
using SpredMedia.Authentication.Core.DTO;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDTO>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(ConfirmEmailDTO => ConfirmEmailDTO.EmailAddress)
                .EmailAddress();
            RuleFor(ConfirmEmailDTO => ConfirmEmailDTO.Token)
                .NotEmpty().WithMessage("must not be empty string")
                .NotNull().WithMessage("must not be null value");
        }
    }
}
