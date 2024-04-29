using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
namespace ProLink.Application.Mail
{
    public class MailingService:IMailingService
    {
        #region fields
        private readonly MailSettings _mailSettings;
        #endregion
        #region ctor
        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        #endregion
        #region methods
        
        public void SendMail(MailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(MailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _mailSettings.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private void Send(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_mailSettings.SmtpServer, _mailSettings.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_mailSettings.Username, _mailSettings.Password);
                    client.Send(message);
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
        #endregion
    }
}
