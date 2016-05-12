using AsiaMoneyer.Account;
using AsiaMoneyer.Category;
using AsiaMoneyer.EventBus;
using AsiaMoneyer.Project.Dtos;
using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.AuditLog;
using AsiaMoneyer.Messaging;
using AsiaMoneyer.SystemSettings;

namespace AsiaMoneyer.Project
{
    public class RecurringTransactionAppService: AppServiceBase, IRecurringTransactionAppService
    {        
        private readonly IEventPublisher _eventPublisher;

        public RecurringTransactionAppService(IEventPublisher eventPublisher)
        {
            this._eventPublisher = eventPublisher;
        }

    }
}
