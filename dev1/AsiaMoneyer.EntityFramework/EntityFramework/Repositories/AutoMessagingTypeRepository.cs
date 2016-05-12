using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Messaging;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class AutoMessagingTypeRepository : AsiaMoneyerRepositoryBase<Entities.AutoMessagingType, int>, IAutoMessagingTypeRepository
    {
        public AutoMessagingTypeRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
