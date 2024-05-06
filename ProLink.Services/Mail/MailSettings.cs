namespace ProLink.Application.Mail
{
    public class MailSettings
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
