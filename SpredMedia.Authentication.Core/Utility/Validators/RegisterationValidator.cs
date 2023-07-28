using FluentValidation;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class RegisterationValidator : AbstractValidator<RegisterationDto>
    {
        public RegisterationValidator()
        {
            RuleFor(RegistrationDTO => RegistrationDTO.Password)
                .Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("Passwords do not match");
            RuleFor(RegistrationDTO => RegistrationDTO.FirstName)
                .HumanName();
            RuleFor(RegistrationDTO => RegistrationDTO.LastName)
                .HumanName();
            RuleFor(RegistrationDTO => RegistrationDTO.PhoneNumber)
                .PhoneNumber();
            RuleFor(RegistrationDTO => RegistrationDTO.Email)
                .EmailAddress();
            RuleFor(RegistrationDTO => RegistrationDTO.Pin)
            .Matches(@"^[0-9]{4}$");
        }
    }
}
