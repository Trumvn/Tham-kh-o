using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class RecurringTransactionDto
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public int TimeFrequencyId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<System.DateTime> GeneratedDate { get; set; }
        public Nullable<System.DateTime> GeneratedToDate { get; set; }
        public int RecurringTimes { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public ProjectDto Project { get; set; }
        public TimeFrequencyDto TimeFrequency { get; set; }
    }
}
