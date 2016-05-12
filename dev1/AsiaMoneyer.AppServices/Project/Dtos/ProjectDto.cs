using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDesc { get; set; }
        public string HighlightColor { get; set; }
        public string WorkingEmail { get; set; }
        public string Currency { get; set; }
        public byte FinanceYearStartMonth { get; set; }
        public byte FinanceYearMonths { get; set; }
        public int ViewFilterByType { get; set; }
        public string ViewFilterByAccount { get; set; }
        public string ViewFilterByCategory { get; set; }
        public Nullable<bool> IsFavorite { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? IsDeleted { get; set; }
        public String UserId { get; set; }
        public Client.Dtos.ClientDto Owner { get; set; }
        public ProjectMemberDto User { get; set; }
        public DateTime CreatedDate { get; set; }

        public TransactionSumDto TransactionSummary { get; set; }
        public BudgetSumDto BudgetExpenseSummary { get; set; }
        public BudgetSumDto BudgetIncomeSummary { get; set; }
    }
}
