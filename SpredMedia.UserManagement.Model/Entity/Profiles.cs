using SpredMedia.CommonLibrary;

namespace SpredMedia.UserManagement.Model.Entity
{
    public class Profile : BaseModel
    {
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }
        public string UserId { get; set; } 
        public User Users { get; set; }
        public IEnumerable<DownloadHistory> Downloads { get; set; }
    }
}
