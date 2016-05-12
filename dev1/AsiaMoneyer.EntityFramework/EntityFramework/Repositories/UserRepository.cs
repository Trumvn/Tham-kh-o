using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.User;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class UserRepository: AsiaMoneyerRepositoryBase<Entities.User>, IUserRepository
    {

        public UserRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
