using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class TransactionSumDto
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Balance { get; set; }
        public decimal Budget { get; set; }
        public int Count { get; set; }
    }
}
