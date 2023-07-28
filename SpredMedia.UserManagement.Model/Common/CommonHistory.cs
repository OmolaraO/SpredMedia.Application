using System;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Model.Common
{
	public class CommonHistory : BaseModel
	{
        public string? UserId { get; set; }
        public string? ProfileId { get; set; }
        public UserProfile? Profile { get; set; }
        public string? MovieTitle { get; set; }
        public string? ContentTye { get; set; }
        public string? Duration { get; set; }
    }
}

