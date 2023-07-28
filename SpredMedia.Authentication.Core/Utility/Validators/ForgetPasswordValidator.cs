using FluentValidation;
using SpredMedia.Authentication.Core.DTO;


namespace SpredMedia.Authentication.Core.Utility.Validators
{
    public class ForgetPasswordValidator : AbstractValidator<ForgotPasswordDTO>
    {
        public ForgetPasswordValidator()
        {
            RuleFor(ForgotPasswordDTO => ForgotPasswordDTO.EmailAddress)
                .EmailAddress();
        }
    }
}
