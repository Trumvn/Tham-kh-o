using AsiaMoneyer.Project;
using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.EntityFramework.Repositories
{

    public class TransactionRepository : AsiaMoneyerRepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
        public List<Transaction> GetTransactionByAcc(string projectId,string accountId)
        {
            List<Transaction> transactions = this.Get(x => x.ProjectId == projectId && x.IsDeleted == false && x.AccountId==accountId).OrderBy(x => x.TransactionDate).ToList();
            return transactions;
        }
        public List<Transaction> GetAvailableTransactions(string projectId)
        {
            List<Transaction> transactions = this.Get(x => x.ProjectId == projectId && x.IsDeleted == false).OrderBy(x=>x.TransactionDate).ToList();
            return transactions;
        }

        public List<Transaction> SearchTransactions(string projectId, string accountId, string categoryId, DateTime fromDate, DateTime endDate, bool isUnclearOnly)
        {
            List<Transaction> transactions = null;
            if(!isUnclearOnly)
            {
                transactions = this.Get(x => x.ProjectId == projectId && x.IsDeleted == false && (x.TransactionDate >= fromDate && x.TransactionDate <= endDate) && (String.IsNullOrEmpty(accountId) || x.AccountId == accountId) && (String.IsNullOrEmpty(categoryId) || x.CategoryId == categoryId)).OrderBy(x => x.TransactionDate).ToList();
            }
            else
            {
                transactions = this.Get(x => x.ProjectId == projectId && x.IsDeleted == false && x.TransactionDate == null && (String.IsNullOrEmpty(accountId) || x.AccountId == accountId) && (String.IsNullOrEmpty(categoryId) || x.CategoryId == categoryId)).OrderBy(x => x.TransactionDate).ToList();
            }
            return transactions;
        }

        public Decimal GetAccountBalance(string accountId, DateTime date)
        {
            decimal currentBalanceIncome = this.List.Where(x => x.AccountId == accountId && x.IsDeleted == false && x.IsClear == true && x.TransactionDate <= date && x.IsIncome == true).Sum(x => x.Amount);
            decimal currentBalanceExpense = this.List.Where(x => x.AccountId == accountId && x.IsDeleted == false && x.IsClear == true && x.TransactionDate <= date && x.IsIncome == false).Sum(x => x.Amount);
            return currentBalanceIncome - currentBalanceExpense;
        }

        public decimal GetBalance(string projectId, DateTime date)
        {
            decimal currentBalanceIncome = this.List.Where(x => x.ProjectId == projectId && x.IsDeleted == false && x.IsClear == true && x.TransactionDate <= date && x.IsIncome == true).Sum(x => x.Amount);
            decimal currentBalanceExpense = this.List.Where(x => x.ProjectId == projectId && x.IsDeleted == false && x.IsClear == true && x.TransactionDate <= date && x.IsIncome == false).Sum(x => x.Amount);
            return currentBalanceIncome - currentBalanceExpense;
        }

        public int SoftDeleteTransactionsInCategory(string categoryId)
        {
            int count = 0;
            List<Transaction> transactions = this.Get(x => x.CategoryId == categoryId && x.IsDeleted == false).ToList();
            if (transactions != null)
            {
                count = transactions.Count;
                foreach (Transaction trans in transactions)
                {
                    trans.IsDeleted = true;
                }
            }
            return count;
        }

        public int SoftDeleteTransactionsInAccount(string accountId)
        {
            int count = 0;
            List<Transaction> transactions = this.Get(x => x.AccountId == accountId && x.IsDeleted == false).ToList();
            if (transactions != null)
            {
                count = transactions.Count;
                foreach (Transaction trans in transactions)
                {
                    trans.IsDeleted = true;
                }
            }
            return count;
        }

        public int ResetRecurringForTransactions(string recurringTransactionId)
        {
            int count = 0;
            List<Transaction> transactions = this.Get(x => x.RecurringTransactionId == recurringTransactionId && x.IsDeleted == false && x.IsClear == true).ToList();
            if (transactions != null)
            {
                count = transactions.Count;
                foreach (Transaction trans in transactions)
                {
                    trans.RecurringTransactionId = null;
                }
            }
            return count;
        }

        public TransactionSumModel GetTransactionBalances(string projectId, DateTime fromDate, DateTime endDate, bool isIncludeUnclearTransaction)
        {
            TransactionSumModel sum = new TransactionSumModel();
            sum.FromDate = fromDate;
            sum.EndDate = endDate;

            List<Transaction> transactions = this.Get(x => x.ProjectId == projectId && x.IsDeleted == false && (isIncludeUnclearTransaction || x.IsClear == true) && x.TransactionDate >= fromDate && x.TransactionDate <= endDate).ToList();
            if (transactions != null)
            {
                foreach(Transaction trans in transactions)
                {
                    if (trans.IsIncome.HasValue && trans.IsIncome.Value)
                        sum.Income += trans.Amount;
                    else
                        sum.Expense += trans.Amount;
                }
                sum.Balance = sum.Income - sum.Expense;
                sum.Count = transactions.Count;
            }

            return sum;
        }

        public TransactionSumModel GetUnclearTransactionBalances(string projectId, DateTime fromDate, DateTime endDate, bool? isIncome)
        {
            TransactionSumModel sum = new TransactionSumModel();
            sum.FromDate = fromDate;
            sum.EndDate = endDate;

            List<Transaction> transactions = this.Get(x => x.ProjectId == projectId && x.IsDeleted == false && x.IsClear == false && (!isIncome.HasValue || (isIncome.HasValue && x.IsIncome == isIncome.Value)) && x.TransactionDate >= fromDate && x.TransactionDate <= endDate).ToList();
            if (transactions != null)
            {
                foreach (Transaction trans in transactions)
                {
                    if (trans.IsIncome.HasValue && trans.IsIncome.Value)
                        sum.Income += trans.Amount;
                    else
                        sum.Expense += trans.Amount;
                }
                sum.Balance = sum.Income - sum.Expense;
                sum.Count = transactions.Count;
            }

            return sum;
        }

        public Decimal GetCategoryBalance(string categoryId, DateTime fromDate, DateTime endDate)
        {

            decimal amount = this.Get(x => x.CategoryId == categoryId && x.IsDeleted == false && x.IsClear == true && x.TransactionDate >= fromDate && x.TransactionDate <= endDate).Sum(x=>x.Amount);

            return amount;
        }

        public Decimal GetCategoryBalanceByAccount(string categoryId, string accountId, DateTime fromDate, DateTime endDate)
        {

            decimal amount = this.Get(x => x.CategoryId == categoryId && x.AccountId == accountId && x.IsDeleted == false && x.IsClear == true && x.TransactionDate >= fromDate && x.TransactionDate <= endDate).Sum(x => x.Amount);

            return amount;
        }

        public Decimal GetAccountBalance(string accountId, DateTime fromDate, DateTime endDate)
        {

            decimal currentBalanceIncome = this.List.Where(x => x.AccountId == accountId && x.IsDeleted == false && x.IsClear == true && x.TransactionDate <= endDate && x.IsIncome == true).Sum(x => x.Amount);
            decimal currentBalanceExpense = this.List.Where(x => x.AccountId == accountId && x.IsDeleted == false && x.IsClear == true && x.TransactionDate <= endDate && x.IsIncome == false).Sum(x => x.Amount);
            return currentBalanceIncome - currentBalanceExpense;
        }

        public TransactionSumModel GetTransactionBalancesByCategory(string categoryId, DateTime fromDate, DateTime endDate, bool isIncludeUnclearTransaction)
        {
            TransactionSumModel sum = new TransactionSumModel();
            sum.FromDate = fromDate;
            sum.EndDate = endDate;

            decimal balance = this.Get(x => x.CategoryId == categoryId && x.IsDeleted == false && (isIncludeUnclearTransaction || x.IsClear == true) && x.TransactionDate >= fromDate && x.TransactionDate <= endDate).Sum(x=>x.Amount);
            sum.Balance = balance;

            return sum;
        }

        public decimal GetTotalIncomeByMonth(string projectId, DateTime fromDate, DateTime endDate)
        {
            decimal balance = this.Get(x => x.ProjectId == projectId && x.IsDeleted == false && x.IsClear == true && x.IsIncome == true && x.TransactionDate >= fromDate && x.TransactionDate <= endDate).Sum(x => x.Amount);

            return balance;
        }

        public string GetTransactionTitle(string transactionId)
        {
            return this.List.Where(x => x.Id == transactionId).Select(x=>x.TransactionTitle).FirstOrDefault();
        }

        public BudgetSumModel GetBudgetSummaryByMonth(string projectId, DateTime fromDate, DateTime endDate, bool isIncome)
        {
            BudgetSumModel budgetSum = new BudgetSumModel();
            budgetSum.StartDate = fromDate;
            budgetSum.EndDate = endDate;
            budgetSum.TimeFrequencyId = (int) Commons.Constants.TIME_FREQUENCY.MONTHLY;

            int weeksInMonth = 4;
            var projectCategories = this.DbContext.CategoryDbSet.Where(x => x.ProjectId == projectId && x.IsIncome == isIncome && x.IsDeleted == false).Select(x=>x.Id);

            List<Entities.CategoryBudget> budgets = this.DbContext.CategoryBudgetDbSet.Where(x => projectCategories.Contains(x.CategoryId) && (x.StartDate == null || x.StartDate <= fromDate) && (x.EndDate == null || x.EndDate >= endDate) && x.IsDeleted == false).ToList();

            List<string> categoryList = new List<string>();
            foreach(Entities.CategoryBudget budget in budgets)
            {
                Entities.TimeFrequency timeFrequency = budget.TimeFrequency;
                budgetSum.BudgetAmount += (decimal)(budget.BudgetAmount * (int)(timeFrequency.Weeks)) / weeksInMonth;
                categoryList.Add(budget.CategoryId);
            }

            // Get actual
            decimal actualBalance = this.List.Where(x => categoryList.Contains(x.CategoryId) && x.IsClear == true && x.IsDeleted == false && x.TransactionDate >= fromDate && x.TransactionDate <= endDate).Sum(x => x.Amount);
            budgetSum.ActualAmount = actualBalance;

            return budgetSum;
        }
    }
}
