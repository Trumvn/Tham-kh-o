using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public interface IAutoMessagingMessageRepository : IRepository<AsiaMoneyer.Entities.AutoMessagingMessage, string>
    {
        List<AutoMessagingMessageModel> GetMessageTitles();
        int CountMessages();
    }
}
