using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class ProjectSearchOutput
    {
        public ProjectFilterDto Filter { get; set; }
        public List<SearchResultProjectDto> Projects { get; set; } 
    }
}
