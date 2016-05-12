using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class BudgetSumDto
    {
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public Dtos.TimeFrequencyDto TimeFrequency { get; set; }

        public decimal Percentage {
            get {
                decimal percentage = 0;
                if(BudgetAmount > 0)
                {
                    percentage = ActualAmount / BudgetAmount;
                }
                return percentage;
            }
        }
    }
}
