using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.UserManagement.Model.Entity
{
    public class ProfileDownloads
    {
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        public string DownloadHistoryId { get; set; }
        public DownloadHistory DownloadHistory { get; set; }
    } 
}
