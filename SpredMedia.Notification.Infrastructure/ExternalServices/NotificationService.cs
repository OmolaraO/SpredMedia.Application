using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SpredMedia.Notification.Core.AppSettings;
using SpredMedia.Notification.Core.Interfaces;
using SpredMedia.Notification.Core.Utilities;
using ILogger = Serilog.ILogger;

namespace SpredMedia.Notification.Infrastructure.ExternalServices
{
	public class NotificationService : INotificationService
	{
        private readonly ILogger _logger;
        private readonly NotificationSettings _notificationSettings;
        private readonly IEmailNotificationProvider _notificationProviders;

        public NotificationService(IServiceProvider provider, IEmailNotificationProvider notificationProviders)
		{
            _logger = provider.GetRequiredService<ILogger>();
            _notificationSettings = provider.GetRequiredService<NotificationSettings>();
            _notificationProviders = notificationProviders;
        }

        public async Task<bool> SendAsync(EmailContext context)
        {
            _logger.Information($"Attempting to fetch details for {JsonConvert.SerializeObject(context.Address)}");
            context.NotificationSettings = _notificationSettings;
            try
            {
                var response = await _notificationProviders.SendSingleAsync(context);

                if (!response)
                {
                    return await Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"notification Error: {context.Address} => {context.Header}");

                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }



        public async Task<bool> SendBulkEmailAsync(BulkMessage message)
        {
            _logger.Information($"Attempting to fetch details for {message}");
            try
            {
                var response = await _notificationProviders.SendBulkAsync(message);

                if (!response)
                {
                    return await Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"notification Error: {message.To} => {message.Subject}");

                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
    }
}

