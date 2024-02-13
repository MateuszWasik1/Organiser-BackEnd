using System.Net.Mail;
using System.Net;

namespace Organiser.Cores.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderSettings settings;
        public EmailSender(EmailSenderSettings settings) => this.settings = settings;

        public void SendEmail(string email, string subject, string message)
        {
            var client = new SmtpClient(settings.Host, settings.Port)
            {
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Login, settings.Password),
            };

            var mail = new MailMessage()
            {
                From = new MailAddress(settings.Login),
                Subject = subject,
                Body = message,
            };

            mail.To.Add(email);

            client.Send(mail);
        }
    }
}