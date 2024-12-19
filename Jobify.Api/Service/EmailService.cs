using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Jobify.Api.Service
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration)
        {
            _smtpHost = configuration["SmtpSettings:Host"]
                ?? throw new ArgumentNullException("SmtpSettings:Host is not configured.");
            _smtpPort = int.TryParse(configuration["SmtpSettings:Port"], out var port) ? port : throw new ArgumentNullException("SmtpSettings:Port is invalid.");
            _smtpUser = configuration["SmtpSettings:User"] ?? throw new ArgumentNullException("SmtpSettings:User is not configured.");
            _smtpPassword = configuration["SmtpSettings:Password"] ?? throw new ArgumentNullException("SmtpSettings:Password is not configured.");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
