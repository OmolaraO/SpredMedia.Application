
using FluentValidation;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequestDTO>
    {
        public RefreshTokenValidator()
        {
            RuleFor(RefreshTokenRequestDTO => RefreshTokenRequestDTO.RefreshToken)
            .IdGuidString();
            RuleFor(RefreshTokenRequestDTO => RefreshTokenRequestDTO.UserId)
            .IdGuidString();
        }
    }
}
