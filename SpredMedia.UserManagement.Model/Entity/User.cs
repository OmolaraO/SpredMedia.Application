using System;
using SpredMedia.CommonLibrary;

namespace SpredMedia.UserManagement.Model.Entity
{
	public class User : BaseModel
	{
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
		public string? EmailAddress { get; set; }
		public string? Password { get; set; }
		public IEnumerable<Profile>? Profiles { get; set; }
        public IEnumerable<Subscription>? Subscriptions { get; set; }
    }
}

