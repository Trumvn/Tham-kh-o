using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.User
{
    public interface IUserRepository : IRepository<AsiaMoneyer.Entities.User, string>
    {
    }
}
