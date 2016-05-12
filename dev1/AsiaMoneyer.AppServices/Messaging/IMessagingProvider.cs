using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public interface IMessagingProvider
    {
        MailMessage send(MailMessage msg);
    }
}
