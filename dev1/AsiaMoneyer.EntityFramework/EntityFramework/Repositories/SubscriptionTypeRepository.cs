using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Billing;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class SubscriptionTypeRepository : AsiaMoneyerRepositoryBase<Entities.SubscriptionType, int>, ISubscriptionTypeRepository
    {
        public SubscriptionTypeRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
