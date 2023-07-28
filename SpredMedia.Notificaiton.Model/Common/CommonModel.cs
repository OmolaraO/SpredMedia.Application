using System;
using SpredMedia.CommonLibrary;
using SpredMedia.Notification.Model.Entity;

namespace SpredMedia.Notification.Model.Common
{
	public class CommonModel : BaseModel
	{
        public string? Body { get; set; }
        public string? Subject { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}

