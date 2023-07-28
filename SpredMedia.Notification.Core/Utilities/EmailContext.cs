using System;
using SpredMedia.Notification.Core.AppSettings;

namespace SpredMedia.Notification.Core.Utilities
{
	public class EmailContext
	{
		public string? Address { get; set; } = string.Empty;
		public string Header { get; set; } = string.Empty;
		public string Payload { get; set; } = null!;
		public NotificationSettings NotificationSettings { get; set; } = null!;
	}
}

