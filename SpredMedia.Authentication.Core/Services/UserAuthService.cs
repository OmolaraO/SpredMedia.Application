using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Core.Utility.Validators;
using SpredMedia.Authentication.Model.Enum;
using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;
using System.Net;

namespace SpredMedia.Authentication.Core.Services
{
    public class UserAuthService : IUserAuthService
    {

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailer _emailer;
        private readonly IDigitalTokenService _digitalTokenService;
        public UserAuthService(IServiceProvider service)
        {
            _userManager = service.GetRequiredService<UserManager<User>>();
            _mapper = service.GetRequiredService<IMapper>();
            _logger = service.GetRequiredService<ILogger>();
            _emailer = service.GetRequiredService<IEmailer>();
            _tokenService = service.GetRequiredService<ITokenService>();
            _unitOfWork = service.GetRequiredService<IUnitOfWork>();
            _digitalTokenService = service.GetRequiredService<IDigitalTokenService>();
        }

        public async Task<ResponseDto<string>> ChangePasswordAsync(ChangePasswordDTO model, string UserId)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserId))
                {
                    _logger.Information("checking the database if the instance exist in the database using the UserId" + UserId);
                    var user = await _userManager.FindByIdAsync(UserId);
                    if (user == null)
                    {
                        return ResponseDto<string>.Fail("User not found.", (int)HttpStatusCode.BadRequest);
                    }
                    _logger.Information("verify if the password is the valid for that user");
                    var isPasswordConfirmed = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                    _logger.Information("verification for this userID" + UserId+ " returns "+  isPasswordConfirmed);
                    if (!isPasswordConfirmed)
                    {
                        return ResponseDto<string>.Fail("Current password is incorrect.", (int)HttpStatusCode.Unauthorized);
                    }
                    _logger.Information("updating the password for the user ID " + UserId);
                    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        _logger.Information("added the password successfully");
                        return ResponseDto<string>.Success("Successfully!", "Password has been updated", (int)HttpStatusCode.Accepted);
                    }
                    _logger.Information("something wrong happend couldnt update the password");
                    return IdentityResultErrors<string>(result);
                }
                return ResponseDto<string>.Fail( "UserId was not passed into the api", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<ResponseDto<string>> ConfirmEmailAsync(ConfirmEmailDTO confirmEmailDTO)
        {
            try
            {
                if(confirmEmailDTO != null)
                {
                    _logger.Information("Queryng the database to check if user in the value is present in the database");
                    var user = await _userManager.FindByEmailAsync(confirmEmailDTO.EmailAddress);
                    if (user == null)
                    {
                        _logger.Information("User not found in the database instances");
                        return ResponseDto<string>.Fail("User not found", (int)HttpStatusCode.NotFound);
                    }
                    var purpose = UserManager<User>.ConfirmEmailTokenPurpose;
                    _logger.Information("THE purpose of email confirmation is " + purpose);
                    var result = await _digitalTokenService.ValidateAsync(purpose, confirmEmailDTO.Token, _userManager, user);
                    _logger.Information("the token has validated to "+ result);
                    if (result)
                    {
                        user.EmailConfirmed = true;
                        user.IsActive = true;
                        var update = await _userManager.UpdateAsync(user);
                        if (update.Succeeded)
                        {
                            _logger.Information("updated succesafully to the database");
                            return ResponseDto<string>.Success("Email Confirmation successful", user.Id, (int)HttpStatusCode.OK);
                        }
                    }
                    return ResponseDto<string>.Fail("Email Confirmation not successful", (int)HttpStatusCode.Unauthorized);
                }
                return ResponseDto<string>.Fail("Email confirm DTO is null", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<bool>> ForgotPasswordAsync(ForgotPasswordDTO ForgotPasswordDTO)
        {
            try
            {
                _logger.Information("running a null check on the database instance");
                if (ForgotPasswordDTO is not null)
                {
                    _logger.Information("checking if the database contains the user instance");
                    var user = await _userManager.FindByEmailAsync(ForgotPasswordDTO.EmailAddress);
                    if (user == null)
                    {
                        _logger.Information("the value of the user is set to null since the instance was not found in the database");
                        return ResponseDto<bool>.Fail("Email does not exist", (int)HttpStatusCode.NotFound);
                    }
                    _logger.Information("getting the purpose to be sent to the database");
                    var purpose =  UserManager<User>.ResetPasswordTokenPurpose;
                    return await _emailer.SendEmail(user, purpose, "ForgotPassword");
                }
                return ResponseDto<bool>.Fail("the object refrence is null", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<CredentialResponseDTO>> LoginAsync(LoginRequestDto model)
        {
            try
            {
                _logger.Information("the user is checked on the database if it exist or not");
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.Information("user doesnt exist or is not a registered customer");
                    return ResponseDto<CredentialResponseDTO>.Fail("User does not exist", (int)HttpStatusCode.NotFound);
                }

                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    _logger.Information("the users Credentials are not valid to proceed futher");
                    return ResponseDto<CredentialResponseDTO>.Fail("Invalid user credential", (int)HttpStatusCode.BadRequest);
                }

                if (!user.EmailConfirmed)
                {
                    _logger.Information("Users EMail has not been confirmed yet");
                    return ResponseDto<CredentialResponseDTO>.Fail("User's account is not confirmed", (int)HttpStatusCode.BadRequest);
                }
                else if (!user.IsActive)
                {
                    _logger.Information("the users has been deactivated  from his account");
                    return ResponseDto<CredentialResponseDTO>.Fail("User's account is deactivated", (int)HttpStatusCode.BadRequest);
                }
                _logger.Information("has been authorized and the access token and also the refresh token are been created");
                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); //sets refresh token for 7 days
                var credentialResponse = new CredentialResponseDTO()
                {
                    Id = user.Id,
                    Token = await _tokenService.GenerateToken(user),
                    RefreshToken = user.RefreshToken
                };
                _logger.Information("Token jas been generated successfully and about to be updated");
                var result = await _userManager.UpdateAsync(user);
                _logger.Information("the Token has been update to the user table");
                if (result.Succeeded)
                {
                    _logger.Information("User successfully logged in");
                    return ResponseDto<CredentialResponseDTO>.Success("Login successful", credentialResponse);
                }
                _logger.Information("Something went Wrong and the user failed to login");
                return ResponseDto<CredentialResponseDTO>.Fail("Failed to login user", (int)HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<RefreshTokenResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken)
        {
            try
            {
                _logger.Information("running a check on the refreshtoken sent to us");
                if (refreshToken is not null)
                {
                    _logger.Information("the refresh token is not invalid");
                    var response = new ResponseDto<RefreshTokenResponseDTO>();
                    var tokenToBeRefreshed = refreshToken.RefreshToken;
                    var userId = refreshToken.UserId;

                    var user = await _userManager.FindByIdAsync(userId);
                    _logger.Information("checking if the access token has exprired before genrating a new access Token");
                    int value = DateTime.Compare((DateTime)user?.RefreshTokenExpiryTime!, DateTime.Now);
                    if (user.RefreshToken != tokenToBeRefreshed || value < 0)
                    {
                        _logger.Information("the token userId sent is not registered under any user");
                        return ResponseDto<RefreshTokenResponseDTO>.Fail("Invalid credentials", (int)HttpStatusCode.BadRequest);
                    }
                    var refreshMapping = new RefreshTokenResponseDTO
                    {
                        NewAccessToken = await _tokenService.GenerateToken(user),
                        NewRefreshToken = _tokenService.GenerateRefreshToken()
                    };
                    _logger.Information("update the user object before adding it to the database");
                    user.RefreshToken = refreshMapping.NewRefreshToken;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                    _logger.Information("ADDed the value to the database usinf the user manager object");
                    await _userManager.UpdateAsync(user); 
                    return ResponseDto<RefreshTokenResponseDTO>.Success("Token refreshed successfully", refreshMapping, (int)HttpStatusCode.Accepted);
                }
                _logger.Information("the value was test to be null");
                return ResponseDto<RefreshTokenResponseDTO>.Fail("the refresh token model is  null", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<RegistrationResponseDto>> RegisterAsync(RegisterationDto model)
        {
            try
            {
                if (model is not null)
                {
                    _logger.Information("the model is not null");
                    _logger.Information("mapping the model parameter into the User Parameter");
                    User user = _mapper.Map<User>(model);
                    _logger.Information("mapping successfully");
                    _logger.Information("checking the email to know if it exist at first");
                    var checkEmail = await _userManager.FindByEmailAsync(user.Email);
                    if (checkEmail != null)
                    {
                        _logger.Information("the email already exist");
                        return ResponseDto<RegistrationResponseDto>.Fail("Email already Exists", (int)HttpStatusCode.BadRequest);
                    }
                    _logger.Information("the email doesnt exist in the database");
                    await _userManager.CreateAsync(user, model.Password);
                    await _userManager.AddToRoleAsync(user, Role.Customer.ToString());
                    // add hangfire service


                    _logger.Information("About to send it for email processing");
                    var sendEmailResponse = await _emailer.SendEmail(user, UserManager<User>.ConfirmEmailTokenPurpose,"ConfirmEmail");
                    var resp = new RegistrationResponseDto { Id = user.Id, Email = user.Email };
                    _logger.Information($"Sending email response: {sendEmailResponse}");    
                    if (sendEmailResponse == null || !sendEmailResponse.Status)
                        return ResponseDto<RegistrationResponseDto>.Success("Registration is successful, but resend otp for email verification",
                                                       resp,sendEmailResponse.StatusCode);
                    _logger.Information("Successfully sent an email for the processing of the email verification");
                    return ResponseDto<RegistrationResponseDto>.Success("Registration Successfully",
                                                                resp,(int)HttpStatusCode.Created);
                }
                _logger.Information("the model is registered as null");
                return ResponseDto<RegistrationResponseDto>.Fail("the model is empty", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<bool>> ResendOTPAsync(ResendOtpDTO ResendOtpDTO)
        {
            try
            {
                if (ResendOtpDTO is not null)
                {
                    _logger.Information("checking if the database contains the user instance");
                    var user = await _userManager.FindByEmailAsync(ResendOtpDTO.Email);
                    if (user == null)
                    {
                        _logger.Information("the value of the user is set to null since the instance was not found in the database");
                        return ResponseDto<bool>.Fail("Email does not exist", (int)HttpStatusCode.NotFound);
                    }
                    _logger.Information("getting the purpose to be sent to the database");
                    var purpose = (ResendOtpDTO.Purpose == "ConfirmEmail") ? UserManager<User>.ConfirmEmailTokenPurpose
                        : UserManager<User>.ResetPasswordTokenPurpose;
                    return await _emailer.SendEmail(user, purpose, ResendOtpDTO.Template);
                }
                return ResponseDto<bool>.Fail("the object refrence is null", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                if (resetPasswordDTO is not null)
                {
                    _logger.Information("Reset password attempt "+ resetPasswordDTO.Email);
                    var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
                    if (user == null)
                    {
                        _logger.Information("the instance for address doesnt exist => "+ resetPasswordDTO.Email);
                        return ResponseDto<string>.Fail("Email does not exist", (int)HttpStatusCode.NotFound);
                    }
                    _logger.Information("the instance for address exist => " + resetPasswordDTO.Email);
                    var purpose = UserManager<User>.ResetPasswordTokenPurpose;
                    var isValidToken = await _digitalTokenService.ValidateAsync(purpose, resetPasswordDTO.Token, _userManager, user);
                    _logger.Information("the validation for => " + resetPasswordDTO.Email + "is "+ isValidToken);
                    var result = new IdentityResult();
                    var hasher = new PasswordHasher<User>();
                    if (isValidToken)
                    {
                        _logger.Information("hashed the password before updating it");
                        var hash = hasher.HashPassword(user, resetPasswordDTO.NewPassword);
                        user.PasswordHash = hash;
                        _logger.Information("About to update the database for => " + resetPasswordDTO.Email);
                        result = await _userManager.UpdateAsync(user);
                    }
                    if (result.Succeeded)
                    {
                        _logger.Information("password has been reset successfully");
                        return ResponseDto<string>.Success("Password has been reset successfully", user.Id, (int)HttpStatusCode.OK);
                    }
                    _logger.Information("invalid token unable to reset password");
                    return ResponseDto<string>.Fail("Invalid Token", (int)HttpStatusCode.BadRequest);
                }
                _logger.Information("the object model is empty");
                return ResponseDto<string>.Fail("Email does not exist", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<CredentialResponseDTO>> VerifyGoogleTokenAsync(GoogleLoginRequestDTO google)
        {
            try
            {
                _logger.Information("carrying out the null exception");
                if (google is not null)
                {
                    var names = google.Name.Split(' ');
                    string firstName = names[0];
                    string lastName = names[1];
                    var role = Role.Customer.ToString();
                    var user = await _userManager.FindByEmailAsync(google.Email);
                    _logger.Information("the user query was carried out for " + google.Name);
                    if (user == null)
                    {
                        _logger.Information($"the user was not found with => {firstName}: {lastName}: {role}");
                        _logger.Information($"Registering the user with {firstName}: {lastName}: {role}");
                        user = new User { Email = google.Email, FirstName = firstName, LastName = lastName, UserName = google.Email };
                        var response = await _userManager.CreateAsync(user);
                        if (!response.Succeeded)
                        {
                            _logger.Error("Could not create external login user");
                            return IdentityResultErrors<CredentialResponseDTO>(response);
                        }
                        response = await _userManager.AddToRoleAsync(user, role);

                        if (!response.Succeeded)
                        {
                            _logger.Error("Could not create roles for login user");
                            return IdentityResultErrors<CredentialResponseDTO>(response);
                        }
                    }
                    _logger.Information("user already exist on the database or the user has just be created in the database");
                    _logger.Information("generating access token for the user with "+ user.Email);
                    user.RefreshToken = _tokenService.GenerateRefreshToken();
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                    user.EmailConfirmed = true;
                    user.IsActive = true;
                    var credentialResponse = new CredentialResponseDTO()
                    {
                        Id = user.Id,
                        Token = await _tokenService.GenerateToken(user),
                        RefreshToken = user.RefreshToken
                    };
                    _logger.Information("saving the user Refresh Token and expiry date to the user identity database");
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.Information("User successfully logged in");
                        return ResponseDto<CredentialResponseDTO>.Success("Login successful", credentialResponse);
                    }
                    _logger.Information("the user was not loggedin");
                    return IdentityResultErrors<CredentialResponseDTO>(result);
                }
                _logger.Information("the application request method is null");
                return ResponseDto<CredentialResponseDTO>.Fail("the request model is null", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        private ResponseDto<T> IdentityResultErrors<T>(IdentityResult result)
        {
            return ResponseDto<T>.Fail(GetErrors(result), (int)HttpStatusCode.InternalServerError);
        }
        private static string GetErrors(IdentityResult result)
        {
            return result.Errors.Aggregate(string.Empty, (curr, err) => curr + err.Description + "\n");
        }
    }
}
