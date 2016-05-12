using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Billing
{
    public interface ISubscriptionRepository : IRepository<AsiaMoneyer.Entities.Subscription, string>
    {
        Entities.Subscription GetUserCurrentSubscription(string userId);
        List<Entities.Subscription> GetLastSubscriptionsByUser(string userId);
        void UpdateLastSubscriptionsByUser(string userId);
    }
}
