using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.AuditLog;
using AsiaMoneyer.Project.Dtos;
using AsiaMoneyer.Entities;
using AsiaMoneyer.Commons;

namespace AsiaMoneyer.Project
{
    public class TransactionAppService : AppServiceBase, ITransactionAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;

        public TransactionAppService(IAuditLogAppService auditLogAppService)
        {
            this._auditLogAppService = auditLogAppService;
        }

        #region Override 
        public Decimal GetCurrentBalanceByAccount(string accountId, DateTime date)
        {
            decimal balance = this.UnitOfWork.TransactionRepository.GetAccountBalance(accountId, date);
            return balance;
        }
        #endregion

        #region Transaction

        private Transaction CreateNewTransaction(TransactionDto transDto)
        {
            var trans = new Transaction()
            {
                Id = transDto.Id,
                AccountId = (transDto.Account != null ? transDto.Account.Id : null),
                CategoryId = (transDto.Category != null ? transDto.Category.Id : null),
                TransactionDate = transDto.TransactionDate,
                TransactionTitle = Commons.Ultility.NormalizeSqlString(transDto.TransactionTitle),
                TransactionDesc = Commons.Ultility.NormalizeSqlString(transDto.TransactionDesc),
                ProjectId = transDto.ProjectId,
                Amount = transDto.Amount,
                IsClear = transDto.IsClear,
                IsIncome = transDto.IsIncome,
                ClientId = transDto.ClientId,
                IsDeleted = transDto.IsDeleted,
                CreatedDate = DateTime.Now
            };

            return trans;
        }

        public List<Dtos.TransactionDto> GetAvailableTransactions(string projectId)
        {
            List<Transaction> transEntities = this.UnitOfWork.TransactionRepository.GetAvailableTransactions(projectId);
            /*await Task.Factory.StartNew(() => {
                transEntities = this._transactionRepository.GetTransactions(projectId);
            });*/
            List<TransactionDto> transDtos = AutoMapper.Mapper.Map<List<Transaction>, List<TransactionDto>>(transEntities);
            return transDtos;
        }
        public List<Dtos.TransactionDto> GetTransactionsByAcc(string projectId, string accountId)
        {
            List<Transaction> transEntities = this.UnitOfWork.TransactionRepository.GetTransactionByAcc(projectId, accountId);
            List<TransactionDto> transDtos = AutoMapper.Mapper.Map<List<Transaction>, List<TransactionDto>>(transEntities);
            return transDtos;
        }
        public List<Dtos.TransactionDto> SearchTransactions(Dtos.TransactionFilterDto filter)
        {
            DateTime queryDate = DateTime.Today;
            if (filter.FromDate.HasValue)
                queryDate = filter.FromDate.Value;

            TimeRange timeRange = DatetimeHelper.GetTimeRange(filter.FilterTypeId, queryDate);

            string categoryId = (filter.Category != null && filter.Category.Id != null) ? filter.Category.Id : String.Empty;
            string accountId = (filter.Account != null && filter.Account.Id != null) ? filter.Account.Id : String.Empty;

            // Reccuring/Repeating
            this.updateRecurringTransaction(filter.ProjectId, timeRange.From, timeRange.End);

            List<Transaction> transEntities = this.UnitOfWork.TransactionRepository.SearchTransactions(filter.ProjectId, accountId, categoryId, timeRange.From, timeRange.End, filter.IsUnclearOnly);

            List<TransactionDto> transDtos = AutoMapper.Mapper.Map<List<Transaction>, List<TransactionDto>>(transEntities);

            return transDtos;
        }

        public async Task CreateTransaction(TransactionDto transDto)
        {
            var trans = this.CreateNewTransaction(transDto);

            await Task.Factory.StartNew(() =>
            {
                this.UnitOfWork.TransactionRepository.Add(trans);
                this.UnitOfWork.Save(this.UserId);
            });
        }

        public Dtos.TransactionDto SaveTransaction(Dtos.TransactionDto transDto)
        {
            Dtos.TransactionDto savedTransDto = null;
            if (!String.IsNullOrEmpty(transDto.Id))
            {
                var trans = new Entities.Transaction()
                {
                    Id = transDto.Id,
                    TransactionTitle = Commons.Ultility.NormalizeSqlString(transDto.TransactionTitle),
                    TransactionDesc = Commons.Ultility.NormalizeSqlString(transDto.TransactionDesc),
                    Amount = transDto.Amount,
                    IsClear = transDto.IsClear,
                    TransactionDate = transDto.TransactionDate,
                    AccountId = (transDto.Account != null ? transDto.Account.Id : null),
                    CategoryId = (transDto.Category != null ? transDto.Category.Id : null),
                    ClientId = (transDto.Client != null ? transDto.Client.Id : null)
                };

                this.UnitOfWork.TransactionRepository.Update(trans, u => u.TransactionTitle, u => u.TransactionDesc, u => u.Amount, u => u.TransactionDate, u => u.IsClear, u => u.CategoryId, u => u.AccountId, u => u.ClientId);
                this.UnitOfWork.Save(this.UserId);

                trans = this.UnitOfWork.TransactionRepository.Get(trans.Id);
                savedTransDto = AutoMapper.Mapper.Map<Entities.Transaction, Dtos.TransactionDto>(trans);

            }
            else
            {
                var trans = new Entities.Transaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    TransactionTitle = Commons.Ultility.NormalizeSqlString(transDto.TransactionTitle),
                    TransactionDesc = Commons.Ultility.NormalizeSqlString(transDto.TransactionDesc),
                    ProjectId = transDto.ProjectId,
                    IsIncome = transDto.IsIncome,
                    Amount = transDto.Amount,
                    TransactionDate = transDto.TransactionDate,
                    AccountId = (transDto.Account != null ? transDto.Account.Id : null),
                    CategoryId = (transDto.Category != null ? transDto.Category.Id : null),
                    ClientId = (transDto.Client != null ? transDto.Client.Id : null),
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                this.UnitOfWork.TransactionRepository.Add(trans);
                this.UnitOfWork.Save(this.UserId);

                trans = this.UnitOfWork.TransactionRepository.Get(trans.Id);
                savedTransDto = AutoMapper.Mapper.Map<Entities.Transaction, Dtos.TransactionDto>(trans);

            }

            return savedTransDto;
        }

        public void DeleteTransaction(Dtos.TransactionDto transDto)
        {
            if (!String.IsNullOrEmpty(transDto.Id))
            {
                var trans = new Entities.Transaction()
                {
                    Id = transDto.Id,
                    IsDeleted = true
                };

                this.UnitOfWork.TransactionRepository.Update(trans, u => u.IsDeleted);
                this.UnitOfWork.Save(this.UserId);
            }
            else
            {
                throw new Exception("Could not delete empty transaction.");
            }
        }

        #endregion

        #region Recurring
        public Dtos.RecurringTransactionDto SaveRecurringTransaction(Dtos.RecurringTransactionSubmitDto recurringTransactionSubmitDto)
        {
            Dtos.RecurringTransactionDto savedRecurringTransDto = null;
            if (!String.IsNullOrEmpty(recurringTransactionSubmitDto.RecurringTransaction.Id))
            {
                var recurringTransaction = new Entities.RecurringTransaction()
                {
                    Id = recurringTransactionSubmitDto.RecurringTransaction.Id,
                    TimeFrequencyId = recurringTransactionSubmitDto.RecurringTransaction.TimeFrequencyId,
                    TransactionDate = recurringTransactionSubmitDto.RecurringTransaction.TransactionDate.Value
                };

                this.removeUnclearTransactionsOfRecurring(recurringTransaction.Id);

                this.UnitOfWork.RecurringTransactionRepository.Update(recurringTransaction, u => u.TimeFrequencyId, u=>u.TransactionDate);
                this.UnitOfWork.Save(this.UserId);

                recurringTransaction = this.UnitOfWork.RecurringTransactionRepository.Get(recurringTransaction.Id);
                savedRecurringTransDto = AutoMapper.Mapper.Map<Entities.RecurringTransaction, Dtos.RecurringTransactionDto>(recurringTransaction);

            }
            else
            {
                var recurringTransaction = new Entities.RecurringTransaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectId = recurringTransactionSubmitDto.RecurringTransaction.ProjectId,
                    TimeFrequencyId = recurringTransactionSubmitDto.RecurringTransaction.TimeFrequencyId,
                    TransactionDate = recurringTransactionSubmitDto.RecurringTransaction.TransactionDate.Value,
                    StartDate = DateTime.Now,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                var trans = new Entities.Transaction()
                {
                    Id = recurringTransactionSubmitDto.TransactionId,
                    RecurringTransactionId = recurringTransaction.Id,
                };

                this.UnitOfWork.RecurringTransactionRepository.Add(recurringTransaction);
                this.UnitOfWork.TransactionRepository.Update(trans, x => x.RecurringTransactionId);

                this.UnitOfWork.Save(this.UserId);

                recurringTransaction = this.UnitOfWork.RecurringTransactionRepository.Get(recurringTransaction.Id);
                savedRecurringTransDto = AutoMapper.Mapper.Map<Entities.RecurringTransaction, Dtos.RecurringTransactionDto>(recurringTransaction);

            }

            return savedRecurringTransDto;
        }

        public void RemoveRecurringTransaction(Dtos.RecurringTransactionSubmitDto recurringTransactionSubmitDto)
        {
            var recurringTransaction = new Entities.RecurringTransaction()
            {
                Id = recurringTransactionSubmitDto.RecurringTransaction.Id,
                IsDeleted = true
            };

            var trans = new Entities.Transaction()
            {
                Id = recurringTransactionSubmitDto.TransactionId,
                RecurringTransactionId = null,
            };

            this.removeUnclearTransactionsOfRecurring(recurringTransaction.Id);

            this.UnitOfWork.TransactionRepository.ResetRecurringForTransactions(recurringTransaction.Id);
            this.UnitOfWork.RecurringTransactionRepository.Update(recurringTransaction, u => u.IsDeleted);
            this.UnitOfWork.Save(this.UserId);

        }

        private void updateRecurringTransaction(string projectId, DateTime fromDate, DateTime endDate)
        {
            List<Entities.RecurringTransaction> recurringTransactions = this.UnitOfWork.RecurringTransactionRepository.List.Where(x => x.ProjectId == projectId && (x.GeneratedToDate == null || x.GeneratedToDate < endDate) && x.IsDeleted == false && (x.EndDate == null || x.EndDate > DateTime.Now)).ToList();

            foreach(Entities.RecurringTransaction retrans in recurringTransactions)
            {
                this.generateTransactionFromRecurringTransaction(retrans, endDate);
            }
            this.UnitOfWork.Save(this.UserId);
        }

        private void generateTransactionFromRecurringTransaction(Entities.RecurringTransaction recurringTransaction, DateTime endDate)
        {
            if(recurringTransaction != null)
            {
                Entities.Transaction sourceTrans = this.GetFirstTransactionFromRecurringTransactions(recurringTransaction.Id);
                if (sourceTrans != null)
                {
                    DateTime fromDate = recurringTransaction.GeneratedToDate ?? recurringTransaction.TransactionDate;
                    List<DateTime> repeatingDates = this.GetRepeatingDates(fromDate, endDate, (Commons.Constants.TIME_FREQUENCY)recurringTransaction.TimeFrequencyId);

                    List<Entities.Transaction> newTransactions = this.GenerateTransactions(sourceTrans, repeatingDates);

                    DateTime lastTransactionDate = DateTime.MaxValue;

                    foreach(Transaction newTrans in newTransactions)
                    {
                        lastTransactionDate = newTrans.TransactionDate.Value;
                        this.UnitOfWork.TransactionRepository.Add(newTrans);
                    }

                    if (lastTransactionDate != DateTime.MaxValue)
                    {
                        recurringTransaction.GeneratedDate = DateTime.Now;
                        recurringTransaction.GeneratedToDate = endDate;
                    }
                }
            }
        }

        private List<DateTime> GetRepeatingDates(DateTime fromDate, DateTime endDate, Commons.Constants.TIME_FREQUENCY frequency)
        {
            List<DateTime> repeatingDates = new List<DateTime>();
            
            int intervalInDays = 0;

            switch(frequency)
            {
                case Commons.Constants.TIME_FREQUENCY.DAILY:
                    intervalInDays = 1;
                    break;
                case Commons.Constants.TIME_FREQUENCY.WEEKLY:
                    intervalInDays = 7;
                    break;
                case Commons.Constants.TIME_FREQUENCY.MONTHLY:
                    intervalInDays = 30;
                    break;
                case Commons.Constants.TIME_FREQUENCY.YEARLY:
                    intervalInDays = 360;
                    break;
                case Commons.Constants.TIME_FREQUENCY.PERIODICALLY:
                    intervalInDays = 0;
                    break;
                case Commons.Constants.TIME_FREQUENCY.ONCE:
                default:
                    intervalInDays = 0;
                    break;
            }

            var tempDate = fromDate;
            while (tempDate <= endDate && intervalInDays > 0) //Less than or Equals means the end date will be added as well
            {
                tempDate = tempDate.AddDays(intervalInDays);
                if (tempDate <= endDate)
                    repeatingDates.Add(tempDate);
            }

            return repeatingDates;
        }

        private Entities.Transaction GetFirstTransactionFromRecurringTransactions(string recurringTransactionId)
        {
            Entities.Transaction trans = this.UnitOfWork.TransactionRepository.List.Where(x => x.RecurringTransactionId == recurringTransactionId && x.IsDeleted == false).OrderBy(x => x.CreatedDate).FirstOrDefault();
            return trans;
        }

        private List<Entities.Transaction> GenerateTransactions(Entities.Transaction sourceTransaction, List<DateTime> repeatingDates)
        {
            List<Entities.Transaction> transactions = new List<Transaction>();
            foreach(DateTime transactionDate in repeatingDates)
            {
                var trans = new Entities.Transaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    TransactionTitle = Commons.Ultility.NormalizeSqlString(sourceTransaction.TransactionTitle),
                    TransactionDesc = Commons.Ultility.NormalizeSqlString(sourceTransaction.TransactionDesc),
                    ProjectId = sourceTransaction.ProjectId,
                    AccountId = sourceTransaction.AccountId,
                    CategoryId = sourceTransaction.CategoryId,
                    IsIncome = sourceTransaction.IsIncome,
                    TransactionDate = transactionDate,
                    IsClear = false,
                    Amount = sourceTransaction.Amount,
                    RecurringTransactionId = sourceTransaction.RecurringTransactionId,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                transactions.Add(trans);
            }

            return transactions;
        }

        private int removeUnclearTransactionsOfRecurring(string recurringTransactionId)
        {
            int count = 0;
            List<Transaction> transactions = this.UnitOfWork.TransactionRepository.List.Where(x => x.RecurringTransactionId == recurringTransactionId && x.IsClear == false).ToList();
            if (transactions != null)
            {
                count = transactions.Count;
                foreach (Entities.Transaction trans in transactions)
                {
                    trans.IsDeleted = true;
                }
            }

            return count;
        }
        #endregion

        #region Summary
        public List<TransactionSumDto> GetTransactionSummaryByMonthYear(Dtos.TransactionFilterDto filter)
        {
            List<TransactionSumDto> trans = new List<TransactionSumDto>();

            if (filter.FromDate.HasValue)
            {
                int financeYearStartMonth = this.UnitOfWork.ProjectRepository.GetProjectFinanceYearStartMonth(filter.ProjectId);

                DateTime today = DateTime.Today;
                DateTime fromDate = new DateTime(filter.FromDate.Value.Year, financeYearStartMonth, 1);
                DateTime endDate = fromDate.AddYears(1).AddDays(-1).AddHours(12);
                int diffMonths = Commons.Ultility.MonthDifference(endDate, fromDate);

                decimal balance = this.UnitOfWork.TransactionRepository.GetBalance(filter.ProjectId, fromDate);
                for (int i = 0; i <= diffMonths; i++)
                {
                    endDate = fromDate.AddMonths(1);

                    TransactionSumDto t = new TransactionSumDto()
                    {
                        Index = i,
                        Title = String.Format("{0}/{1}", fromDate.Month, fromDate.Year),
                    };

                    if (filter.IsUnclearOnly || Commons.Ultility.MonthDifference(today, fromDate) >= 0)
                    {
                        TransactionSumModel transSumModel = this.UnitOfWork.TransactionRepository.GetTransactionBalances(filter.ProjectId, fromDate, endDate, filter.IsUnclearOnly);

                        t.Income = transSumModel.Income;
                        t.Expense = transSumModel.Expense;
                        t.Balance = balance + transSumModel.Balance;

                    }

                    balance = t.Balance;

                    trans.Add(t);

                    fromDate = endDate;
                }
            }
            return trans;
        }

        public List<TransactionSumDto> GetTransactionSummaryByCategory(Dtos.TransactionFilterDto filter)
        {
            List<TransactionSumDto> trans = new List<TransactionSumDto>();

            if (filter.FromDate.HasValue && filter.EndDate.HasValue)
            {
                DateTime fromDate = filter.FromDate.Value;
                DateTime endDate = filter.EndDate.Value;

                double diffDays = Commons.Ultility.DayDifference(endDate, fromDate);

                if(diffDays == 0) // check year
                {
                    int financeYearStartMonth = this.UnitOfWork.ProjectRepository.GetProjectFinanceYearStartMonth(filter.ProjectId);
                    fromDate = new DateTime(filter.FromDate.Value.Year, financeYearStartMonth, 1);
                    endDate = fromDate.AddYears(1).AddDays(-1).AddHours(12);
                }

                string accountId = String.Empty;
                if(filter.Account != null && !String.IsNullOrEmpty(filter.Account.Id))
                {
                    accountId = filter.Account.Id;
                }

                List<Entities.Category> categories = this.UnitOfWork.CategoryRepository.List.Where(x => x.ProjectId == filter.ProjectId && x.IsDeleted == false).ToList();

                foreach (Entities.Category cat in categories)
                {
                    decimal amount = 0;
                    if (!String.IsNullOrEmpty(accountId))
                    {
                        amount = this.UnitOfWork.TransactionRepository.GetCategoryBalanceByAccount(cat.Id, accountId, fromDate, endDate);
                    }
                    else
                    {
                        amount = this.UnitOfWork.TransactionRepository.GetCategoryBalance(cat.Id, fromDate, endDate);
                    }

                    bool isIncome = cat.IsIncome.HasValue && cat.IsIncome.Value;

                    TransactionSumDto t = new TransactionSumDto()
                    {
                        Title = String.Format("{0}", cat.CategoryTitle),
                        Income = isIncome ? amount : 0,
                        Expense = isIncome ? 0 : amount,
                        Balance = amount,
                    };

                    trans.Add(t);
                }
            }

            return trans;
        }

        public List<TransactionSumDto> GetTransactionSummaryByAccount(Dtos.TransactionFilterDto filter)
        {
            List<TransactionSumDto> trans = new List<TransactionSumDto>();

            if (filter.FromDate.HasValue && filter.EndDate.HasValue)
            {
                DateTime fromDate = filter.FromDate.Value;
                DateTime endDate = filter.EndDate.Value;

                double diffDays = Commons.Ultility.DayDifference(endDate, fromDate);

                if (diffDays == 0) // check year
                {
                    int financeYearStartMonth = this.UnitOfWork.ProjectRepository.GetProjectFinanceYearStartMonth(filter.ProjectId);
                    fromDate = new DateTime(filter.FromDate.Value.Year, financeYearStartMonth, 1);
                    endDate = fromDate.AddYears(1).AddDays(-1).AddHours(12);
                }

                List<Entities.Account> accounts = this.UnitOfWork.AccountRepository.List.Where(x => x.ProjectId == filter.ProjectId && x.IsDeleted == false).ToList();

                foreach (Entities.Account acc in accounts)
                {
                    decimal amount = this.UnitOfWork.TransactionRepository.GetAccountBalance(acc.Id, fromDate, endDate);

                    TransactionSumDto t = new TransactionSumDto()
                    {
                        Title = String.Format("{0}", acc.AccountTitle),
                        Balance = amount,
                    };

                    trans.Add(t);
                }
            }

            return trans;
        }

        public List<TransactionSumDto> LoadTransactionCategorySummaryByMonthYear(Dtos.TransactionFilterDto filter)
        {
            List<TransactionSumDto> trans = new List<TransactionSumDto>();

            if (filter.Category != null && !String.IsNullOrEmpty(filter.Category.Id) && filter.FromDate.HasValue)
            {
                int financeYearStartMonth = this.UnitOfWork.ProjectRepository.GetProjectFinanceYearStartMonth(filter.ProjectId);
                DateTime fromDate = new DateTime(filter.FromDate.Value.Year, financeYearStartMonth, 1);
                DateTime endDate = fromDate.AddYears(1).AddDays(-1).AddHours(12);
                int diffMonths = Commons.Ultility.MonthDifference(endDate, fromDate);

                for (int i = 0; i <= diffMonths; i++)
                {
                    endDate = fromDate.AddMonths(1);

                    TransactionSumModel transSumModel = this.UnitOfWork.TransactionRepository.GetTransactionBalancesByCategory(filter.Category.Id, fromDate, endDate, filter.IsUnclearOnly);

                    decimal budget = this.UnitOfWork.CategoryBudgetRepository.GetCategoryMonthlyBudget(filter.Category.Id, fromDate, endDate);

                    bool? isIncomeCategory = this.UnitOfWork.CategoryRepository.List.Where(x => x.Id == filter.Category.Id).Select(x => x.IsIncome).FirstOrDefault();
                    bool bIncomeCategory = (isIncomeCategory.HasValue && isIncomeCategory.Value) ? true : false;

                    TransactionSumDto t = new TransactionSumDto()
                    {
                        Index = i,
                        Title = String.Format("{0}/{1}", fromDate.Month, fromDate.Year),
                        Income = (bIncomeCategory? budget : 0),
                        Expense = (!bIncomeCategory ? budget : 0),
                        Budget = budget,
                        Balance = transSumModel.Balance,
                    };

                    trans.Add(t);

                    fromDate = endDate;
                }
            }
            return trans;
        }

        public ProjectAnalyseInformationDto LoadProjectAnalyseInformation(Dtos.TransactionFilterDto filter)
        {
            ProjectAnalyseInformationDto projectAnalyseInformation = new ProjectAnalyseInformationDto();

            if(filter != null && !String.IsNullOrEmpty(filter.ProjectId))
            {
                int financeYearStartMonth = this.UnitOfWork.ProjectRepository.GetProjectFinanceYearStartMonth(filter.ProjectId);
                DateTime fromDate = new DateTime(DateTime.Today.Year, financeYearStartMonth, 1);
                DateTime endDate = fromDate.AddYears(1).AddDays(-1).AddHours(12);
                int diffMonths = Commons.Ultility.MonthDifference(endDate, fromDate);

                projectAnalyseInformation.AnalyseDate = Commons.Ultility.GetLastDateOfMonth(DateTime.Now);
                projectAnalyseInformation.FinanceYearFromDate = fromDate;
                projectAnalyseInformation.FinanceYearEndDate = endDate;
                projectAnalyseInformation.Currency = this.UnitOfWork.ProjectRepository.GetProjectCurrency(filter.ProjectId);

                // Uncompleted transactions
                bool? isIncome = null;
                TransactionSumModel transSumModel = this.UnitOfWork.TransactionRepository.GetUnclearTransactionBalances(filter.ProjectId, fromDate, projectAnalyseInformation.AnalyseDate, isIncome);

                projectAnalyseInformation.UncompletedTransaction = new TransactionSumDto()
                {
                    Income = transSumModel.Income,
                    Expense = transSumModel.Expense,
                    Balance = transSumModel.Balance,
                    Count = transSumModel.Count,
                };

                // Upcoming income transactions
                isIncome = true;
                transSumModel = this.UnitOfWork.TransactionRepository.GetUnclearTransactionBalances(filter.ProjectId, projectAnalyseInformation.AnalyseDate, projectAnalyseInformation.FinanceYearEndDate, isIncome);

                projectAnalyseInformation.UpcomingIncome = new TransactionSumDto()
                {
                    Income = transSumModel.Income,
                    Expense = transSumModel.Expense,
                    Balance = transSumModel.Balance,
                    Count = transSumModel.Count,
                };

                // Upcoming expense transactions
                isIncome = false;
                transSumModel = this.UnitOfWork.TransactionRepository.GetUnclearTransactionBalances(filter.ProjectId, projectAnalyseInformation.AnalyseDate, projectAnalyseInformation.FinanceYearEndDate, isIncome);

                projectAnalyseInformation.UpcomingExpense = new TransactionSumDto()
                {
                    Income = transSumModel.Income,
                    Expense = transSumModel.Expense,
                    Balance = transSumModel.Balance,
                    Count = transSumModel.Count,
                };

            }

            return projectAnalyseInformation;
        }

        public decimal GetTotalIncomeByMonth(string projectId, DateTime month)
        {
            DateTime fromDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = Commons.Ultility.GetLastDateOfMonth(month);

            decimal totalIncome = this.UnitOfWork.TransactionRepository.GetTotalIncomeByMonth(projectId, fromDate, endDate);
            return totalIncome;
        }
        #endregion

        #region Transfer Money
        public void TransferMoney(Project.Dtos.TransferMoneyDto transferMoneyDto)
        {
            if (transferMoneyDto == null)
                throw new NullReferenceException("TransactionMoneyDto cannot be null");

            if (transferMoneyDto.AccountFrom == null)
                throw new NullReferenceException("TransferMoneyDto.AccountFrom cannot be null");

            if (transferMoneyDto.AccountTo == null)
                throw new NullReferenceException("TransferMoneyDto.AccountTo cannot be null");

            if (String.Compare(transferMoneyDto.AccountFrom.Id, transferMoneyDto.AccountTo.Id, true) == 0)
                throw new NullReferenceException("Accounts must be different");

            if (transferMoneyDto.Amount <= 0)
                throw new NullReferenceException("TransferMoneyDto.Amount cannot be Zero");

            var transOut = new Entities.Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                TransactionTitle = "Transfer Money",
                TransactionDesc = "",
                ProjectId = transferMoneyDto.ProjectId,
                IsIncome = false,
                Amount = transferMoneyDto.Amount,
                TransactionDate = transferMoneyDto.TransferDate,
                AccountId = transferMoneyDto.AccountFrom.Id,
                IsClear = transferMoneyDto.IsClear,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            };

            this.UnitOfWork.TransactionRepository.Add(transOut);

            if (transferMoneyDto.Fee > 0)
            {
                var transFee = new Entities.Transaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    TransactionTitle = "Transfer Money Fee",
                    TransactionDesc = "",
                    ProjectId = transferMoneyDto.ProjectId,
                    IsIncome = false,
                    Amount = transferMoneyDto.Fee,
                    TransactionDate = transferMoneyDto.TransferDate,
                    AccountId = transferMoneyDto.AccountFrom.Id,
                    IsClear = transferMoneyDto.IsClear,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };
                this.UnitOfWork.TransactionRepository.Add(transFee);
            }

            var transIn = new Entities.Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                TransactionTitle = "Transfer Money",
                TransactionDesc = "",
                ProjectId = transferMoneyDto.ProjectId,
                IsIncome = true,
                Amount = transferMoneyDto.Amount,
                TransactionDate = transferMoneyDto.TransferDate,
                AccountId = transferMoneyDto.AccountTo.Id,
                IsClear = transferMoneyDto.IsClear,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            };

            this.UnitOfWork.TransactionRepository.Add(transIn);

            this.UnitOfWork.Save(this.UserId);
        }

        #endregion
    }
}
