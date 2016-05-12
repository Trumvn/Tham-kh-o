using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.Messaging
{
    public interface IAutoMessagingAppService : IAppService
    {
        Dtos.AutoMessagingMessageDto SendEmail(String templateId, Dictionary<string, string> values);
    }
}
