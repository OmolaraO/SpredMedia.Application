using System;
namespace SpredMedia.Notification.Core.AppSettings
{
	public class NotificationSettings
	{
		public string? Host { get; set; }
		public int Port { get; set; }
		public string? From { get; set; }
		public string? SenderTitle { get; set; }
		public string? ReceiverTitle { get; set; }
		public string? Password { get; set; }
	}
}

