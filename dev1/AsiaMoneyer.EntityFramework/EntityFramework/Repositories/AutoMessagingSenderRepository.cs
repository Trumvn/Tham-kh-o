using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Messaging;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class AutoMessagingSenderRepository : AsiaMoneyerRepositoryBase<Entities.AutoMessagingSender, int>, IAutoMessagingSenderRepository
    {
        public AutoMessagingSenderRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
