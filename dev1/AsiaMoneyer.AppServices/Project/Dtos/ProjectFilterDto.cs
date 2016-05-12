using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class ProjectFilterDto
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Commons.Constants.PROJECT_FILTER FilterBy { get; set; }
    }
}
