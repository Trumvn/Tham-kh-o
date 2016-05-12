using AsiaMoneyer.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class AccountRepository : AsiaMoneyerRepositoryBase<Entities.Account>, IAccountRepository
    {
        public AccountRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public List<Entities.Account> GetAccounts(string projectId)
        {
            List<Entities.Account> accounts = this.Get(x=>x.ProjectId == projectId && x.IsDeleted == false).ToList();
            return accounts;
        }

        public List<Entities.Account> GetAvailableAccounts(string projectId)
        {
            List<Entities.Account> accounts = this.Get(x => x.ProjectId == projectId && x.IsClosed == false && x.IsDeleted == false).ToList();
            return accounts;
        }

        public void SetAccountPrimary(string accountId)
        {
            Entities.Account account = this.Get(x => x.Id == accountId).FirstOrDefault();
            if(account != null)
            {
                account.IsPrimary = true;
                List<Entities.Account> accountsInProject = this.Get(x => x.ProjectId == account.ProjectId && x.IsPrimary == true).ToList();
                accountsInProject.ForEach(x => x.IsPrimary = false);
            }
        }
        public string GetAccountTitle(string accountId)
        {
            Entities.Account account = this.Get(x=>x.Id==accountId).FirstOrDefault();
            return account.AccountTitle;
        }
    }
}
