using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using SpredMedia.Notification.Core.AppSettings;
using SpredMedia.Notification.Core.Interfaces;

namespace SpredMedia.Notification.Core.Utilities
{
    public class EmailNotificationProvider : IEmailNotificationProvider
    {
        public readonly NotificationSettings _notificationSettings;

        public EmailNotificationProvider(IServiceProvider provider)
        {
            _notificationSettings = provider.GetRequiredService<NotificationSettings>();
        }

        public async Task<bool> SendSingleAsync(EmailContext emailcontext)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_notificationSettings.From));
                email.To.Add(MailboxAddress.Parse(emailcontext.Address));
                email.Subject = emailcontext.Header;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = emailcontext.Payload
                };
                using var smtp = new SmtpClient();
                //await smtp.ConnectAsync(_notificationSettings.Host, _notificationSettings.Port, SecureSocketOptions.StartTls);
                await smtp.ConnectAsync(_notificationSettings.Host, _notificationSettings.Port, true);
                await smtp.AuthenticateAsync(_notificationSettings.From, _notificationSettings.Password);
                var value = await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return await Task.FromResult(false);
            }
        }




        public async Task<bool> SendBulkAsync(BulkMessage message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("email", _notificationSettings.From));
                email.To.AddRange(message.To);
                email.Subject = message.Subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                {
                    Text = message.Message
                };
                using var client = new SmtpClient();
                client.Connect(_notificationSettings.Host, _notificationSettings.Port, SecureSocketOptions.StartTls);
                //client.Connect(_notificationSettings.Host, _notificationSettings.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_notificationSettings.From, _notificationSettings.Password);
                var value = client.Send(email);
                client.Disconnect(true);
                client.Dispose();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await Task.FromResult(false);
            }
        }
    }
}

