using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Messaging;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class AutoMessagingTemplateRepository : AsiaMoneyerRepositoryBase<Entities.AutoMessagingTemplate>, IAutoMessagingTemplateRepository
    {
        public AutoMessagingTemplateRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
