

using FluentValidation;
using SpredMedia.Authentication.Core.DTO;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class EndpointValidator : AbstractValidator<EndpointRequestDto>
    {
        public EndpointValidator()
        {
            RuleFor(EndpointRequestDto => EndpointRequestDto.Channel)
                .NotEmpty().WithMessage("should not be empty").NotNull().WithMessage("should not be null")
                .Matches(@"^[0-9]$").WithMessage("should be an integer that must start from 0 - 9");
            RuleFor(EndpointRequestDto => EndpointRequestDto.Endpoint)
                .NotEmpty().WithMessage("should not contain values").NotNull().WithMessage("should not be null");
            RuleFor(EndpointRequestDto => EndpointRequestDto.ControllerName)
                .NotEmpty().WithMessage("should not contain values").NotNull().WithMessage("should not be null");
        }
    }
}
