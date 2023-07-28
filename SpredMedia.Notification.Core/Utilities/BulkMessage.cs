using System;
using MimeKit;
using SpredMedia.Notification.Core.DTOs;

namespace SpredMedia.Notification.Core.Utilities
{
	public class BulkMessage
	{
		public List<MailboxAddress> To { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }

		public BulkMessage(IEnumerable<string> to, string subject, string content)
		{
            try
            {
                To = to.Select(x => new MailboxAddress("", x)).ToList();
                Subject = subject;
                Message = content;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            

        }
    }
}

