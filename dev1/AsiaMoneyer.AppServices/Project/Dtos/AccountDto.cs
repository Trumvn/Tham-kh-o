using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class AccountDto
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string AccountTitle { get; set; }
        public string AccountDescription { get; set; }
        public string HighlightColor { get; set; }
        public decimal CurrentBalance { get; set; }
        public Nullable<bool> IsPrimary { get; set; }
        public Nullable<bool> IsClosed { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int Stage { get; set; }
    }
}
