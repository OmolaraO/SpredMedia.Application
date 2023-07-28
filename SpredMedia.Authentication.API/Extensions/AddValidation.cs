using FluentValidation;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Core.Utility.Validators;

namespace SpredMedia.Authentication.API.Extensions
{
    public static class AddValidation
    {
        public static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<RegisterationDto>, RegisterationValidator>();
            services.AddTransient<IValidator<ClientRequestDto>, ClientValidator>();
            services.AddTransient<IValidator<EndpointRequestDto>, EndpointValidator>();
            services.AddTransient<IValidator<ChangePasswordDTO>, ChangePasswordValidator>();
            services.AddTransient<IValidator<ConfirmEmailDTO>, ConfirmEmailValidator>();
            services.AddTransient<IValidator<ForgotPasswordDTO>, ForgetPasswordValidator>();
            services.AddTransient<IValidator<GoogleLoginRequestDTO>, GoogleLoginRequestValidator>();
            services.AddTransient<IValidator<LoginRequestDto>, LoginRequestValidator>();
            services.AddTransient<IValidator<RefreshTokenRequestDTO>, RefreshTokenValidator>();
            services.AddTransient<IValidator<ResendOtpDTO>, ResendOtpValidator>();
            services.AddTransient<IValidator<ResetPasswordDTO>, ResetPasswordValidator>();
            services.AddTransient<IValidator<KeyAndIv>, KeyAndIvValidator>();
        }
    }
}
