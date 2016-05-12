using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AsiaMoneyer.Security.Services {
    public class EmailService : IIdentityMessageService
    {
        
        private const string MAIL_ACCOUNT = "backary.team@gmail.com";
        private const string MAIL_PASSWORD = "123456789x@X";
        
        public async Task SendAsync(IdentityMessage message)
        {
            var gmailClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(MAIL_ACCOUNT, MAIL_PASSWORD)
            };
            var msg = new MailMessage(MAIL_ACCOUNT, message.Destination, message.Subject, message.Body);
            await gmailClient.SendMailAsync(msg);
        }
        
    }
}
