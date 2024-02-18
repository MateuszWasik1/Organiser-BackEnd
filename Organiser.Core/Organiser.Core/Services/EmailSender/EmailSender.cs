using MimeKit;
using MimeKit.Text;

namespace Organiser.Cores.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderSettings settings;
        public EmailSender(EmailSenderSettings settings) => this.settings = settings;

        public void SendEmail(string email, string subject, string message)
        {
            var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress("Sender Name", settings.Login));
            mail.To.Add(new MailboxAddress("Receiver Name", email));

            mail.Subject = subject;
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = $"<b>Hello all the way from the land of C# + {message}</b>"
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect(settings.Host, settings.Port, true);

                smtp.Authenticate(settings.Login, settings.Password);

                smtp.Send(mail);
                smtp.Disconnect(true);
            }
        }
    }
}