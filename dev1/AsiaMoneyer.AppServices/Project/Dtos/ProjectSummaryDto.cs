using AsiaMoneyer.AuditLog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class ProjectSummaryDto
    {
        public ProjectHeaderDto Project {get; set;}
        public Client.Dtos.ClientDto Owner { get; set; }
    }
}
