using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.User
{
    public interface IClientRepository : IRepository<AsiaMoneyer.Entities.Client, string>
    {
        List<AsiaMoneyer.Entities.Client> GetClients();
        List<Entities.UserClientModel> GetClients(int pageIndex, int pageSize);
    }
}
