using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Billing
{
    public interface ISubscriptionTypeRepository : IRepository<AsiaMoneyer.Entities.SubscriptionType, int>    
    {
    }
}
