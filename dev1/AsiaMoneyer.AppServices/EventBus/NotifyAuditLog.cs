using AsiaMoneyer.AuditLog;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.EventBus
{
    public class NotifyAuditLog : IConsumer<TransactionChangedEvent>
    {
        public ILogger Logger { get; set; }

        private readonly IAuditLogAppService _auditLogAppService;

        public NotifyAuditLog(IAuditLogAppService auditLogAppService)
        {
            this._auditLogAppService = auditLogAppService;
        }

        public void Handle(TransactionChangedEvent eventMessage)
        {
            // Update AuditLog here
            if(eventMessage.ActionCode == TransactionChangedEvent.TRANSACTION_CHANGED_EVENT_CODE.Create)
            {
                Logger.Info(String.Format("Transaction Changed Event: /Create/{0}", eventMessage.TransactionId));
            }
            else
            {
                Logger.Info(String.Format("Transaction Changed Event: /Modify/{0}", eventMessage.TransactionId));
            }            
        }
    }
}
