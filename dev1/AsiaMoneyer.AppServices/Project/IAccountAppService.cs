using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project
{
    public interface IAccountAppService : IAppService
    {
        Dtos.ProjectAccountDto GetAccounts(string projectId);
        Dtos.AccountDto SaveAccount(Dtos.AccountDto accountDtos);
        void setAccountPrimary(string accountId);
        void setAccountClosed(string accountId, bool isPrimary);
        Dtos.ProjectAccountDto GetAccountCurrentBalance(string projectId);
        void SoftDeleteAccount(string accountId);
    }
}
