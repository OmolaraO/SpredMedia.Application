using System;
using SpredMedia.CommonLibrary;

namespace SpredMedia.UserManagement.Model.Entity
{
	public class UserSubscription
	{
		public string? UserId { get; set; }
        public string? SubscriptionId { get; set; }
		public User? User { get; set; }
		public Subscription? Subscription { get; set; }
	}
}

