using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.DTO
{
    public class EmailNotificationDTO
    {
        public string UserId { get; set; }
        public string ToRecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
