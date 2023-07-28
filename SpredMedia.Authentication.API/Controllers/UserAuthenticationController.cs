using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Core.Interface;
using System.Net.Mime;
using System.Security.Claims;

namespace SpredMedia.Authentication.API.Controllers
{
    [Route("AuthenticationMS/api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;
        public UserAuthenticationController(IServiceProvider serviceProvider)
        {
            _userAuthService = serviceProvider.GetRequiredService<IUserAuthService>();  
        }

        /// <summary>
        /// used to register the user
        /// for the spred application
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterationDto model)
        {
            var result = await _userAuthService.RegisterAsync(model);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Resent OTP for confirmation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("resend-user-otp")]
        public async Task<IActionResult> ResendOTP([FromBody] ResendOtpDTO model)
        {
            var response = await _userAuthService.ResendOTPAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Verify email with token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("confirm-user-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailDTO request)
        {
            var response = await _userAuthService.ConfirmEmailAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Authenticate a user that has access to our application
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var response = await _userAuthService.LoginAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// External login with Google Authentication
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("user-google-login")]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GoogleLogin(GoogleLoginRequestDTO request)
        {
            var response = await _userAuthService.VerifyGoogleTokenAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Generate a reset token and send to user email address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("forget-user-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var response = await _userAuthService.ForgotPasswordAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Reset password of a logged out user
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        [HttpPost("reset-user-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _userAuthService.ResetPasswordAsync(resetPasswordDTO);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Change password of a logged in user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("change-user-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var userId = HttpContext.User.FindFirst(user => user.Type == ClaimTypes.NameIdentifier).Value;
            var response = await _userAuthService.ChangePasswordAsync(model, userId);

            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Refresh token of a logged in user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("refresh-user-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO model)
        {
            var response = await _userAuthService.RefreshTokenAsync(model);
            return StatusCode(response.StatusCode, response);
        }

    }
}
