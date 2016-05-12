using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class TransactionFilterDto
    {
        public string ProjectId { get; set; }
        public Commons.Constants.TRANSACTION_FILTER FilterTypeId { get; set; }
        public bool IsIncome { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsUnclearOnly { get; set; }
        public CategoryDto Category { get; set; }
        public AccountDto Account { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
    }
}
