
using FluentValidation;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class ClientValidator : AbstractValidator<ClientRequestDto>
    {
        public ClientValidator()
        {
            RuleFor(ClientRequestDto => ClientRequestDto.ClientPassword)
                .Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.ClientPassword)
                .WithMessage("Passwords do not match");
            RuleFor(ClientRequestDto => ClientRequestDto.ClientUsername)
                .HumanName();
            RuleFor(ClientRequestDto => ClientRequestDto.CompanyPhone)
                .PhoneNumber();
            RuleFor(ClientRequestDto => ClientRequestDto.CompanyEmail)
                .EmailAddress();
        }
    }
}
