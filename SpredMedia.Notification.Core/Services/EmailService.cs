using System;
using System.Data.Common;
using System.Net;
using System.Reflection.Metadata;
using System.Transactions;
using MimeKit;
using SpredMedia.CommonLibrary;
using SpredMedia.Notification.Core.DTOs;
using SpredMedia.Notification.Core.Interfaces;
using SpredMedia.Notification.Core.Utilities;
using SpredMedia.Notification.Model.Entity;
using ILogger = Serilog.ILogger;

namespace SpredMedia.Notification.Core.Services
{
	public class EmailService : IEmailService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IEmailNotificationProvider _provider;

        public EmailService(ILogger logger, IUnitOfWork unitOfWork, INotificationService notificationService, IEmailNotificationProvider provider)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _provider = provider;
        }

        public async Task<ResponseDto<bool>> SendBulkEmailToSpecific(BulkEmailDTO dto)
        {
            bool result;

            try
            {
                if (dto.RecipientAddresses != null)
                {
                    var message = new BulkMessage(dto.RecipientAddresses, dto.Subject, dto.Message);
                    //message.Subject = dto.Subject;
                    //message.Message = dto.Message;
                    //message.To = (List<MailboxAddress>)dto.RecipientAddresses.OfType<MailboxAddress>();

                    result = await _notificationService.SendBulkEmailAsync(message);

                    if (!result)
                        return ResponseDto<bool>.Fail("Could not send email to Spred Users", (int)HttpStatusCode.ServiceUnavailable);
                }
                else
                {
                    _logger.Information($"No spred user's email addresses provided");
                    return ResponseDto<bool>.Fail($"No recipient Email Address", (int)HttpStatusCode.NotFound);
                }

                Model.Entity.Email emaildetail = new()
                {
                    RecipientAddresses = dto.RecipientAddresses,
                    Subject = dto.Subject,
                    Body = dto.Message,
                };
                await _unitOfWork.Email.InsertAsync(emaildetail);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.Information("Unable to initiate email sending");
                return ResponseDto<bool>.Fail($"Unable to initiate notification:{ex.Message}", (int)HttpStatusCode.Unauthorized);
            }
            return ResponseDto<bool>.Success("Successfully sent email to Spred users provided", true, (int)HttpStatusCode.OK);
        }



        public async Task<ResponseDto<bool>> SendBulkEmailToAll(BulkEmailToAllDTO dto)
        {
            bool result;

            try
            {
                var addresses = await _unitOfWork.User.GetAllUserAddresses();

                if (addresses != null)
                {
                    var message = new BulkMessage(addresses, dto.Subject, dto.Message);
                    //message.Subject = dto.Subject;
                    //message.Message = dto.Message;
                    //message.To.AddRange(addresses.OfType<MailboxAddress>());

                    result = await _notificationService.SendBulkEmailAsync(message);

                    if (!result)
                        return ResponseDto<bool>.Fail("Could not send email to All Spred Users", (int)HttpStatusCode.ServiceUnavailable);
                }
                else
                {
                    _logger.Information($"No spred user's address found in database");
                    return ResponseDto<bool>.Fail($"No recipient Email Address", (int)HttpStatusCode.NotFound);
                }

                Model.Entity.Email emaildetail = new()
                {
                    RecipientAddresses = addresses,
                    Subject = dto.Subject,
                    Body = dto.Message,
                };
                await _unitOfWork.Email.InsertAsync(emaildetail);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.Information("Unable to initiate email sending");
                return ResponseDto<bool>.Fail($"Unable to initiate notification:{ex.Message}", (int)HttpStatusCode.Unauthorized);
            }
            return ResponseDto<bool>.Success("Successfully sent email to all Spred users", true, (int)HttpStatusCode.OK);
        }


        public async Task<ResponseDto<bool>> SendSingleEmailAtAddress(SingleEmailDTO dto)
        {
            bool result;

            var emailcontext = new EmailContext();
            try
            {
               if (dto.ToRecipientEmail != null)
                {
                    // Log info
                    _logger.Information($"Attempting to fetch recipient email address input");
                    emailcontext = new EmailContext
                    {
                        Header = dto.Subject,
                        Address = dto.ToRecipientEmail,
                        Payload = dto.Message
                    };
                    result = await _notificationService.SendAsync(emailcontext);
                    if (!result)
                        return ResponseDto<bool>.Fail("Could not send email", (int)HttpStatusCode.ServiceUnavailable);
                }
                else
                {
                    _logger.Information($"No recipient email address provided");
                    return ResponseDto<bool>.Fail($"No recipient Email Address", (int)HttpStatusCode.NotFound);
                }

                Model.Entity.Email emaildetail = new()
                {
                    Subject = dto.Subject,
                    Body = dto.Message,
                    RecipientAddress = dto.ToRecipientEmail,
                };
                //await _unitOfWork.Email.InsertAsync(emaildetail);
                //await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.Information("Unable to initiate email sending");
                return ResponseDto<bool>.Fail($"Unable to initiate notification:{ex.Message}", (int)HttpStatusCode.Unauthorized);
            }

            return ResponseDto<bool>.Success("Successfully sent email", true, (int)HttpStatusCode.OK);
        }


        public async Task<ResponseDto<bool>> SendSingleEmail(SingleEmailDTO dto)
        {
            bool result;

            var emailcontext = new EmailContext();
            try { 
                var user = await _unitOfWork.User.GetUserById(dto.UserId);

                if (user != null)
                {
                    emailcontext = new EmailContext
                    {
                        Header = dto.Subject,
                        Address = user.EmailAddress,
                        Payload = dto.Message
                    };
                    result = await _notificationService.SendAsync(emailcontext);
                    if (!result)
                        return ResponseDto<bool>.Fail("Could not send email", (int)HttpStatusCode.ServiceUnavailable);
                }
                else
                {
                    _logger.Information($"No user with id {dto.UserId} in database");
                    return ResponseDto<bool>.Fail($"No recipient Email Address", (int)HttpStatusCode.NotFound);
                }

                Model.Entity.Email emaildetail = new()
                {
                    User = user,
                    UserId = dto.UserId,
                    Subject = dto.Subject,
                    Body = dto.Message,
                    RecipientAddress = user.EmailAddress,
                };
                await _unitOfWork.Email.InsertAsync(emaildetail);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.Information("Unable to initiate email sending");
                return ResponseDto<bool>.Fail($"Unable to initiate notification:{ex.Message}", (int)HttpStatusCode.Unauthorized);
            }

            return ResponseDto<bool>.Success("Successfully sent email", true, (int)HttpStatusCode.OK);
        }
    }
}

