using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Commons;
using AsiaMoneyer.Project.Dtos;

namespace AsiaMoneyer.Project
{
    public class AccountAppService : AppServiceBase, IAccountAppService
    {
        #region Override
        public ProjectAccountDto GetAccounts(string projectId)
        {
            ProjectAccountDto projectAccountDto = new ProjectAccountDto();
            projectAccountDto.ProjectId = projectId;
            List<AsiaMoneyer.Entities.Account> accountEntities = this.UnitOfWork.AccountRepository.GetAccounts(projectId);
            projectAccountDto.Accounts = AutoMapper.Mapper.Map<List<Entities.Account>, List<Dtos.AccountDto>>(accountEntities);
            return projectAccountDto;
        }

        public ProjectAccountDto GetAccountCurrentBalance(string projectId)
        {
            ProjectAccountDto projectAccountDto = new ProjectAccountDto();
            projectAccountDto.ProjectId = projectId;

            List<AsiaMoneyer.Entities.Account> accountEntities = this.UnitOfWork.AccountRepository.GetAccounts(projectId);
            projectAccountDto.Accounts = AutoMapper.Mapper.Map<List<Entities.Account>, List<Dtos.AccountDto>>(accountEntities);
            foreach (Dtos.AccountDto account in projectAccountDto.Accounts)
            {
                account.CurrentBalance = this.UnitOfWork.TransactionRepository.GetAccountBalance(account.Id, DateTime.Now);
            }
            return projectAccountDto;

        }

        public Dtos.AccountDto SaveAccount(Dtos.AccountDto accountDto)
        {
            Dtos.AccountDto savedAccountDto = null;
            if (!String.IsNullOrEmpty(accountDto.Id))
            {
                var account = new Entities.Account()
                {
                    Id = accountDto.Id,
                    AccountTitle = Commons.Ultility.NormalizeSqlString(accountDto.AccountTitle),
                    AccountDescription = Commons.Ultility.NormalizeSqlString(accountDto.AccountDescription),
                    HighlightColor = accountDto.HighlightColor,
                };

                this.UnitOfWork.AccountRepository.Update(account, u => u.AccountTitle, u => u.AccountDescription, u => u.HighlightColor);

                this.UnitOfWork.Save(this.UserId);

                account = this.UnitOfWork.AccountRepository.Get(account.Id);
                savedAccountDto = AutoMapper.Mapper.Map<Entities.Account, Dtos.AccountDto>(account);
            }
            else
            {
                var account = new Entities.Account()
                {
                    Id = Guid.NewGuid().ToString(),
                    AccountTitle = Commons.Ultility.NormalizeSqlString(accountDto.AccountTitle),
                    AccountDescription = Commons.Ultility.NormalizeSqlString(accountDto.AccountDescription),
                    ProjectId = accountDto.ProjectId,
                    HighlightColor = accountDto.HighlightColor,
                    IsPrimary = false,
                    IsClosed = false,
                    IsDeleted = false,
                    OpenDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };

                this.UnitOfWork.AccountRepository.Add(account);
                this.UnitOfWork.Save(this.UserId);

                account = this.UnitOfWork.AccountRepository.Get(account.Id);
                savedAccountDto = AutoMapper.Mapper.Map<Entities.Account, Dtos.AccountDto>(account);
            }

            return savedAccountDto;
        }

        public void setAccountPrimary(string accountId)
        {
            if (!String.IsNullOrEmpty(accountId))
            {
                // Set to primary
                this.UnitOfWork.AccountRepository.SetAccountPrimary(accountId);

                this.UnitOfWork.Save(this.UserId);
            }
            else
            {
                throw new Exception("Account ID could not be empty");
            }

        }


        public void setAccountClosed(string accountId, bool isClosed)
        {
            if (!String.IsNullOrEmpty(accountId))
            {
                var account = new Entities.Account()
                {
                    Id = accountId,
                    IsClosed = isClosed
                };

                this.UnitOfWork.AccountRepository.Update(account, u => u.IsClosed);

                this.UnitOfWork.Save(this.UserId);
            }
            else
            {
                throw new Exception("Account ID could not be empty");
            }

        }

        public void SoftDeleteAccount(string accountId)
        {
            if (!String.IsNullOrEmpty(accountId))
            {
                var account = new Entities.Account()
                {
                    Id = accountId,
                    IsDeleted = true,
                };

                this.UnitOfWork.AccountRepository.Update(account, u => u.IsDeleted);

                this.UnitOfWork.TransactionRepository.SoftDeleteTransactionsInAccount(accountId);

                this.UnitOfWork.Save(this.UserId);
            }
            else
            {
                throw new Exception(Constants.EXCEPTION_MESSAGE_NOT_FOUND);
            }
        }
        #endregion

        #region Private


        #endregion
    }
}
