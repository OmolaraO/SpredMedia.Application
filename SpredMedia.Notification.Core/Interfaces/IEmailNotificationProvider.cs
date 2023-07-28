using System;
using SpredMedia.Notification.Core.Utilities;

namespace SpredMedia.Notification.Core.Interfaces
{
	public interface IEmailNotificationProvider
	{
		Task<bool> SendSingleAsync(EmailContext context);

		Task<bool> SendBulkAsync(BulkMessage message);

    }
}

