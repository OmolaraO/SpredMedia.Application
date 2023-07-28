using System;
using SpredMedia.CommonLibrary;
using SpredMedia.Notification.Model.Common;

namespace SpredMedia.Notification.Model.Entity
{
	public class Email : CommonModel
	{
		public string? RecipientAddress { get; set; }
        public List<string>? RecipientAddresses { get; set; }
        public string? SenderAddress { get; set; }
	}
}

