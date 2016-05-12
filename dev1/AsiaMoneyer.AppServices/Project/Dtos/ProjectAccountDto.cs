using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class ProjectAccountDto
    {
        public string ProjectId { get; set; }
        public List<Dtos.AccountDto> Accounts { get; set; }
    }
}
