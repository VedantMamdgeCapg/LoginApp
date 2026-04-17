using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using LoginApp.Services;
using Microsoft.Extensions.Options;

namespace LoginApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        private readonly IWebHostEnvironment _environment;

        public EmailSender(IOptions<EmailSettings> options, IWebHostEnvironment environment)
        {
            _settings = options.Value;
            _environment = environment;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.From),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            message.To.Add(new MailAddress(email));

            using var client = new SmtpClient();

            if (_settings.UsePickupDirectory)
            {
                var pickupDirectory = _settings.PickupDirectoryLocation;
                if (!Path.IsPathRooted(pickupDirectory))
                {
                    pickupDirectory = Path.Combine(_environment.ContentRootPath, pickupDirectory);
                }

                Directory.CreateDirectory(pickupDirectory);

                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = pickupDirectory;
            }
            else
            {
                client.Host = _settings.Host;
                client.Port = _settings.Port;
                client.EnableSsl = _settings.EnableSsl;

                if (!string.IsNullOrWhiteSpace(_settings.UserName))
                {
                    client.Credentials = new NetworkCredential(_settings.UserName, _settings.Password);
                }
            }

            return client.SendMailAsync(message);
        }
    }
}
