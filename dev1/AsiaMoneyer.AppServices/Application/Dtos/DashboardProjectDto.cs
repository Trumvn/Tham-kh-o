using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Dtos
{
    public class DashboardProjectDto
    {
        public int? Id { get; set; }
        public string ProjectTitle { get; set; }
        public string Description { get; set; }
        public decimal AverageIncome { get; set; }
        public decimal AverageExpense { get; set; }
        public decimal TotalCash { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? IsArchive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
