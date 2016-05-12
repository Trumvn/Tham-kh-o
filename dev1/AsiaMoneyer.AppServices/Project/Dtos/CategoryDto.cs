using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ParentId { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryDescription { get; set; }
        public string HighlightColor { get; set; }
        public Nullable<bool> IsIncome { get; set; }
        public Nullable<bool> IsClosed { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public CategoryBudgetDto CurrentBudget { get; set; }
        //public List<CategoryBudgetDto> CategoryBudgets { get; set; }

        public CategoryDto Parent { get; set; }
        public List<CategoryDto> Childs { get; set; }
    }
}
