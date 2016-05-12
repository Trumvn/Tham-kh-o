using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Dtos
{
    public class NavigatorProjectDto
    {
        public string Id { get; set; }
        public string ProjectTitle { get; set; }
        public Project.Dtos.ProjectMemberDto User { get; set; }
        public bool? IsPrivate { get; set; }
    }
}
