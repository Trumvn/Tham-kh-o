using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public class GoogleEmailProvider : AppServiceBase, IMessagingProvider
    {
        public MailMessage send(MailMessage msg)
        {
            Entities.AutoMessagingSender sender = this.GetMessagingSenderCredential((int)Commons.Constants.AUTO_MESSAGING_SENDER.GOOGLE);

            if (sender != null)
            {

                string providerServerUrl = sender.ProviderHost;
                int providerServerPort = sender.ProviderPort;
                string username = sender.CredentialUserName;
                string passworkHash = sender.CredentialPasswordHash;
                string securityStamp = sender.SecurityStamp;
                string password = Commons.Crypto.DecryptStringAES(passworkHash, securityStamp);
                bool enableSsl = sender.ProviderEnableSsl;

                if(msg.From == null)
                {
                    string displayName = sender.DisplayName;
                    msg.From = new MailAddress(username, displayName);
                }

                System.Net.Mail.SmtpClient server = new System.Net.Mail.SmtpClient(providerServerUrl);
                server.Credentials = new System.Net.NetworkCredential(username, password);
                server.EnableSsl = enableSsl;
                server.Port = providerServerPort;
                server.Send(msg);
            }
            else
            {
                throw new Exception("No sender found!");
            }

            return msg;
        }

        private Entities.AutoMessagingSender GetMessagingSenderCredential(int senderId)
        {
            Entities.AutoMessagingSender sender = this.UnitOfWork.AutoMessagingSenderRepository.Get(senderId);
            return sender;
        }
    }
}
