using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Serilog;
using Microsoft.Extensions.Configuration;
using SpredMedia.Authentication.Core.AppSettings;
using Newtonsoft.Json;

namespace SpredMedia.Authentication.Core.Services
{
    public class Emailer : IEmailer
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly IDigitalTokenService _digitTokenService;
        private readonly IAuthClientService _authClientService;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly NotificationSettings _notificationSettings;
        public Emailer(IServiceProvider service)
        {
            _notificationSettings =  service.GetRequiredService<NotificationSettings>();
            _userManager = service.GetRequiredService<UserManager<User>>();
            _tokenService = service.GetRequiredService<ITokenService>();
            _digitTokenService = service.GetRequiredService<IDigitalTokenService>();
            _authClientService = service.GetService<IAuthClientService>();
            _logger = service.GetRequiredService<ILogger>();
            _config = service.GetRequiredService<IConfiguration>();
            _httpContextAccessor = service.GetRequiredService<IHttpContextAccessor>() ?? throw new ArgumentNullException(nameof(IHttpContextAccessor)); ;
        }
        public async Task<ResponseDto<bool>> SendEmail(User userModel, string purpose,string template)
        {
            _logger.Information("Enter the Emailer service");
            _logger.Information("getting the information from the token service");
            string token = await _digitTokenService.GenerateAsync(purpose, _userManager, userModel);
            _logger.Information($"about to retrieve the email template for sending the email");
            var mailBody = await GetEmailBody(userModel, $"StaticFolder/{template}.html", token);
            var sendEmail = new EmailNotificationDTO
            {
                UserId = userModel.Id,
                ToRecipientEmail = userModel.Email,
                Subject = "Email Verification",
                Message = mailBody
            };
            _logger.Information("encrypting body message and the Bearer Token to send to the gateway");
            var encryptedBody = Encrypter.Encode(JsonConvert.SerializeObject(sendEmail), _httpContextAccessor.HttpContext,_logger, _config);
            var encrytedBearToken = Encrypter.Encode(_notificationSettings.EmailAccessToken, _httpContextAccessor.HttpContext, _logger, _config);
            try
            {
                return   await _authClientService.PostRequestAsync<ResponseDto<bool>>
                            (_notificationSettings.BaseUrl, _notificationSettings.BaseUrl+_notificationSettings.SendToSingleEmail, encryptedBody,
                             encrytedBearToken);

            }
            catch (Exception)
            {
                return ResponseDto<bool>.Fail("Service is not available, please try again later.",
                    (int)HttpStatusCode.ServiceUnavailable);
            }
        }

        private async Task<string> GetEmailBody(User user, string emailTempPath, string token)
        {
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            var userFirstName = textInfo.ToTitleCase(user.FirstName ?? "");
            _logger.Information("About to get the static email file");
            var temp = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), emailTempPath));
            _logger.Information($"Successfully get email path: {temp}");
            return temp.Replace("**code**", token).Replace("**user**", userFirstName);
        }
    }
}
