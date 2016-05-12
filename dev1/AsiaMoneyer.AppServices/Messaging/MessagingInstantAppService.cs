using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public class MessagingInstantAppService : AppServiceBase, IAutoMessagingAppService
    {
        private readonly IAutoMessagingMessageAppService _autoMessagingMessageAppService;
        IMessagingProvider _messagingProvider;

        public MessagingInstantAppService(IMessagingProvider messagingProvider, IAutoMessagingMessageAppService autoMessagingMessageAppService)
        {
            this._messagingProvider = messagingProvider;
            this._autoMessagingMessageAppService = autoMessagingMessageAppService;
        }

        public Dtos.AutoMessagingMessageDto SendEmail(String templateId, Dictionary<string, string> values)
        {
            Dtos.AutoMessagingMessageDto message = this._autoMessagingMessageAppService.SendEmail(templateId, values);
            this.SendEmail(message.MessagingTo, message.MessagingSubject, message.MessagingContent);
            this.UpdateMessagingMessage(message);
            return message;
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
            IAutoMailMessage<MailMessage> mail = new AutoSmptEmailMessage();

            mail.AddToEmailAddress(toEmail);

            mail.Subject = subject;
            mail.Body = body;

            MailMessage mailMessage = _messagingProvider.send(mail.MailMessage);
        }

        private void UpdateMessagingMessage(Dtos.AutoMessagingMessageDto message)
        {
            AutoMessagingMessage msg = new AutoMessagingMessage() {
                Id = message.Id,
                IsSent = true,
                SentDate = DateTime.Now
            };

            this.UnitOfWork.AutoMessagingMessageRepository.Update(msg, x => x.IsSent, x => x.SentDate);
            this.UnitOfWork.Save();
        }
    }
}
