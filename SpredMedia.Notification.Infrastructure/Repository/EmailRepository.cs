using System;
using SpredMedia.Notification.Core.Interfaces;
using SpredMedia.Notification.Model.Entity;

namespace SpredMedia.Notification.Infrastructure.Repository
{
	public class EmailRepository : GenericRepository<Email>, IEmailRepository
	{
		public EmailRepository(NotificationDbContext context) : base(context)
		{

		}
	}
}

