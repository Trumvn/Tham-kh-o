using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Messaging;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class AutoMessagingDataMappingRepository : AsiaMoneyerRepositoryBase<Entities.AutoMessagingDataMapping>, IAutoMessagingDataMappingRepository
    {
        public AutoMessagingDataMappingRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
