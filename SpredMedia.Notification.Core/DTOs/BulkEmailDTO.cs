using System;
using System.ComponentModel.DataAnnotations;

namespace SpredMedia.Notification.Core.DTOs
{
	public class BulkEmailDTO
	{
        public List<string>? RecipientAddresses { get; set; }
        public string Subject { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
    }
}

