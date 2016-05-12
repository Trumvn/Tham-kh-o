using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Billing;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class SubscriptionRepository : AsiaMoneyerRepositoryBase<Entities.Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public Entities.Subscription GetUserCurrentSubscription(string userId)
        {
            DateTime today = DateTime.Today.AddDays(1);
            return this.List.Where(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false && x.ValidDate <= today && (x.ExpiredDate == null || x.ExpiredDate > today)).FirstOrDefault();
        }

        public List<Entities.Subscription> GetLastSubscriptionsByUser(string userId)
        {
            return this.List.Where(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false).ToList();
        }

        public void UpdateLastSubscriptionsByUser(string userId)
        {
            List<Entities.Subscription> subscriptions = this.GetLastSubscriptionsByUser(userId);

            foreach(Entities.Subscription subscription in subscriptions )
            {
                subscription.IsActive = false;
                this.Update(subscription, x => x.IsActive);
            }
        }
    }
}
