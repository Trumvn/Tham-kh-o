using AsiaMoneyer.Project.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project
{
    public interface ITransactionAppService : IAppService
    {
        Decimal GetCurrentBalanceByAccount(string accountId, DateTime date);

        List<Dtos.TransactionDto> GetAvailableTransactions(string projectId);
        List<Dtos.TransactionDto> SearchTransactions(Dtos.TransactionFilterDto filter);
        Task CreateTransaction(Dtos.TransactionDto transDto);
        Dtos.TransactionDto SaveTransaction(Dtos.TransactionDto transDto);
        void DeleteTransaction(Dtos.TransactionDto transDto);
        List<TransactionSumDto> GetTransactionSummaryByMonthYear(Dtos.TransactionFilterDto filter);
        List<TransactionSumDto> GetTransactionSummaryByCategory(Dtos.TransactionFilterDto filter);
        List<TransactionSumDto> GetTransactionSummaryByAccount(Dtos.TransactionFilterDto filter);
        List<TransactionSumDto> LoadTransactionCategorySummaryByMonthYear(Dtos.TransactionFilterDto filter);
        ProjectAnalyseInformationDto LoadProjectAnalyseInformation(Dtos.TransactionFilterDto filter);

        decimal GetTotalIncomeByMonth(string projectId, DateTime month);

        Dtos.RecurringTransactionDto SaveRecurringTransaction(Dtos.RecurringTransactionSubmitDto recurringTransactionSubmitDto);
        void RemoveRecurringTransaction(Dtos.RecurringTransactionSubmitDto recurringTransactionSubmitDto);

        void TransferMoney(Project.Dtos.TransferMoneyDto transferMoneyDto);
    }
}
