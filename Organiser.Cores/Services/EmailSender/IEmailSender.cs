namespace Organiser.Cores.Services.EmailSender
{
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string message);
    }
}
