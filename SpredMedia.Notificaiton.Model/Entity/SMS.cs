
using SpredMedia.Notification.Model.Common;

namespace SpredMedia.Notification.Model.Entity
{
	public class SMS : CommonModel
	{
        public string? RecipientNumber { get; set; }
        public string? SenderNumber { get; set; }
    }
}

