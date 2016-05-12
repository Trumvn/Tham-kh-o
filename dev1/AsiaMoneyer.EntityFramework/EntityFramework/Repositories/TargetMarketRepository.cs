using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Billing;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class TargetMarketRepository : AsiaMoneyerRepositoryBase<Entities.TargetMarket, int>, ITargetMarketRepository
    {
        public TargetMarketRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
