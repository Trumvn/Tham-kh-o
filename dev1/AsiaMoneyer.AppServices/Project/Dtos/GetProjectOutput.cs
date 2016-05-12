using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class GetProjectsOutput
    {
        public ProjectFilterDto Filter { get; set; }
        public List<ProjectDto> Projects { get; set; } 
    }
}
