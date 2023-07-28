using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.AppSettings
{
    public class NotificationSettings
    {
        public string BaseUrl { get; set; }
        public string EmailAccessToken { get; set; }
        public string SendToSingleEmail { get; set; }
    }
}
