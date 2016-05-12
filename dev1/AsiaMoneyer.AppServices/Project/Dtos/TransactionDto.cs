using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class TransactionDto 
    {
        public string Id { get; set; }
        public string TransactionTitle { get; set; }
        public string TransactionDesc { get; set; }
        public string ProjectId { get; set; }
        public string AccountId { get; set; }
        public string CategoryId { get; set; }
        public string RecurringTransactionId { get; set; }
        public Nullable<int> BudgetId { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public Nullable<bool> IsIncome { get; set; }
        public string Payee { get; set; }
        public Nullable<bool> IsClear { get; set; }
        public string ClientId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public Dtos.AccountDto Account { get; set; }
        public Dtos.CategoryDto Category { get; set; }
        public Dtos.RecurringTransactionDto RecurringTransaction { get; set; }
        public Client.Dtos.ClientDto Client { get; set; }
    }
}
