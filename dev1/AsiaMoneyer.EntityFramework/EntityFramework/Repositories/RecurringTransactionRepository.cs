using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Project;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class RecurringTransactionRepository : AsiaMoneyerRepositoryBase<Entities.RecurringTransaction>, IRecurringTransactionRepository
    {
        public RecurringTransactionRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
    
        }    
    }
}
