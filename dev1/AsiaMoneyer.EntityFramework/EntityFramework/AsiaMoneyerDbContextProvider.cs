using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.EntityFramework
{
    public class AsiaMoneyerDbContextProvider : IDbContextProvider<AsiaMoneyerDbContext>
    {
        protected AsiaMoneyerDbContext dbContext;
        public AsiaMoneyerDbContext DbContext
        {
            get
            {
                if (dbContext == null)
                {
                    dbContext = new AsiaMoneyerDbContext();
                }
                return dbContext;
            }
        }
    }
}
