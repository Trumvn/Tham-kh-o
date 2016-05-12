using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class TransferMoneyDto
    {
        public string ProjectId { get; set; }
        public Dtos.AccountDto AccountFrom { get; set; }
        public Dtos.AccountDto AccountTo { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Currency { get; set; }
        public DateTime TransferDate { get; set; }
        public bool IsClear { get; set; }
    }
}
