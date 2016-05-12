using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.EventBus
{
    public class TransactionChangedEvent
    {
        public enum TRANSACTION_CHANGED_EVENT_CODE
        {
            Create = 1,
            Modify,
        }
        public string TransactionId { get; set; }
        public TRANSACTION_CHANGED_EVENT_CODE ActionCode { get; set; }
    }
}
