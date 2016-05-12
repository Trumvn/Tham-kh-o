using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class ProjectHeaderDto
    {
        public string Id { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDesc { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Currency { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? IsArchive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
