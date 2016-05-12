using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.Project
{
    public interface ITransactionRepository : IRepository<Transaction, string>
    {
        List<Transaction> GetAvailableTransactions(string projectId);
        List<Transaction> SearchTransactions(string projectId, string accountId, string categoryId, DateTime fromDate, DateTime endDate, bool isUnclearOnly);
        List<Transaction> GetTransactionByAcc(string projectId,string accountId);

        TransactionSumModel GetTransactionBalances(string projectId, DateTime fromDate, DateTime endDate, bool isIncludeUnclearTransaction);
        TransactionSumModel GetUnclearTransactionBalances(string projectId, DateTime fromDate, DateTime endDate, bool? isIncome);
        TransactionSumModel GetTransactionBalancesByCategory(string categoryId, DateTime fromDate, DateTime endDate, bool isIncludeUnclearTransaction);

        BudgetSumModel GetBudgetSummaryByMonth(string projectId, DateTime fromDate, DateTime endDate, bool isIncome);

        Decimal GetAccountBalance(string accountId, DateTime date);
        Decimal GetAccountBalance(string accountId, DateTime fromDate, DateTime endDate);
        Decimal GetCategoryBalance(string categoryId, DateTime fromDate, DateTime endDate);
        Decimal GetCategoryBalanceByAccount(string categoryId, string accountId, DateTime fromDate, DateTime endDate);
        decimal GetBalance(string projectId, DateTime date);
        decimal GetTotalIncomeByMonth(string projectId, DateTime fromDate, DateTime endDate);
        int SoftDeleteTransactionsInCategory(string categoryId);
        int SoftDeleteTransactionsInAccount(string accountId);

        int ResetRecurringForTransactions(string recurringTransactionId);
        string GetTransactionTitle(string transactionId);
    }
}
