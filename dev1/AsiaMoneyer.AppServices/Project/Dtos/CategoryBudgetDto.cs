using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class CategoryBudgetDto
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public Nullable<int> TimeFrequencyId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public decimal BudgetAmount { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Dtos.TimeFrequencyDto TimeFrequency { get; set; }
        public Dtos.CategoryDto Category { get; set; }

        public string Currency { get; set; }
    }
}
