namespace ProLink.Application.Mail
{
    public interface IMailingService
    {
        void SendMail(MailMessage message);
    }
}
