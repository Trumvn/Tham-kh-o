using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.Messaging
{
    public class MessagingQueueAppService : AppServiceBase, IAutoMessagingAppService
    {
        private readonly IAutoMessagingMessageAppService autoMessagingMessageAppService;

        public MessagingQueueAppService(IAutoMessagingMessageAppService autoMessagingMessageAppService)
        {
            this.autoMessagingMessageAppService = autoMessagingMessageAppService;
        }

        public Dtos.AutoMessagingMessageDto SendEmail(string templateId, Dictionary<string, string> values)
        {
            return this.autoMessagingMessageAppService.SendEmail(templateId, values);
        }
    }
}
