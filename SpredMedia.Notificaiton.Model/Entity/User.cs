using System;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Notification.Model.Entity
{
	public class User : BaseModel
	{
		public string? UserName { get; set; }
		public string? EmailAddress { get; set; } 
		public string? PhoneNumber { get; set; }
		public IEnumerable<Email>? Email { get; set; }
		public IEnumerable<SMS>? SMS { get; set; }
	}
}

