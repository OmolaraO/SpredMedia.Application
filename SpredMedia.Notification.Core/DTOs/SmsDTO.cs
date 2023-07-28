using System;
using System.ComponentModel.DataAnnotations;

namespace SpredMedia.Notification.Core.DTOs
{
	public class SmsDTO
	{
        [EmailAddress]
        [RegularExpression("^[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?", ErrorMessage = "Invalid email format!")]
        public string ToRecipientNumber { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
    }
}

