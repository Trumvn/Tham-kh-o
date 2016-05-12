using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class RecurringTransactionSubmitDto
    {
        public string TransactionId { get; set; }
        public RecurringTransactionDto RecurringTransaction {get; set; }
    }
}
