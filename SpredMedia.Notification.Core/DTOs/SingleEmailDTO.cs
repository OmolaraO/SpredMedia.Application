using System;
using System.ComponentModel.DataAnnotations;

namespace SpredMedia.Notification.Core.DTOs
{
	public class SingleEmailDTO
	{
        [EmailAddress]
        [RegularExpression("^[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?", ErrorMessage = "Invalid email format!")]
        public string ToRecipientEmail { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
        public string UserId { get; set; } = String.Empty;
    }
}

