using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Account
{
    public interface IAccountRepository : IRepository<AsiaMoneyer.Entities.Account, string>
    {
        List<AsiaMoneyer.Entities.Account> GetAccounts(string projectId);
        List<AsiaMoneyer.Entities.Account> GetAvailableAccounts(string projectId);
        void SetAccountPrimary(string accountId);
        string GetAccountTitle(string accountId);
    }
}
