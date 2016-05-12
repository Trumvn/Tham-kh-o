using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class ProjectAnalyseInformationDto
    {
        public string ProjectId { get; set; }
        public DateTime AnalyseDate { get; set; }
        public DateTime FinanceYearFromDate { get; set; }
        public DateTime FinanceYearEndDate { get; set; }
        public TransactionSumDto UncompletedTransaction { get; set; }
        public TransactionSumDto UpcomingIncome { get; set; }
        public TransactionSumDto UpcomingExpense { get; set; }
        public string Currency { get; set; }
    }
}
