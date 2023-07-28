using System;
using SpredMedia.Notification.Core.Utilities;

namespace SpredMedia.Notification.Core.Interfaces
{
	public interface INotificationService
	{
        Task<bool> SendAsync(EmailContext context);

        Task<bool> SendBulkEmailAsync(BulkMessage message);

    }

}

