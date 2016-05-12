using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CategoryBudgets")]
    public class CategoryBudget : Entity<string>, Base.IAuditable
    {
        public CategoryBudget()
        {
            
        }

        public string CategoryId { get; set; }
        public Nullable<int> TimeFrequencyId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public decimal BudgetAmount { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        [ForeignKey("TimeFrequencyId")]
        public virtual TimeFrequency TimeFrequency { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
