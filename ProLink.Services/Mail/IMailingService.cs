using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.Mail
{
    public interface IMailingService
    {
        void SendMail(MailMessage message);
    }
}
