

using SpredMedia.Authentication.Core.DTO;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Interface
{
    public interface IUserAuthService
    {
        Task<ResponseDto<RegistrationResponseDto>> RegisterAsync(RegisterationDto model);
        Task<ResponseDto<bool>> ResendOTPAsync(ResendOtpDTO model);
        Task<ResponseDto<string>> ConfirmEmailAsync(ConfirmEmailDTO request);
        Task<ResponseDto<CredentialResponseDTO>> LoginAsync(LoginRequestDto model);
        Task<ResponseDto<CredentialResponseDTO>> VerifyGoogleTokenAsync(GoogleLoginRequestDTO request);
        Task<ResponseDto<bool>> ForgotPasswordAsync(ForgotPasswordDTO model);
        Task<ResponseDto<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<ResponseDto<string>> ChangePasswordAsync(ChangePasswordDTO request, string UserId);
        Task<ResponseDto<RefreshTokenResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO model);

    }
}
