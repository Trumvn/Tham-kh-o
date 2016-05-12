using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class SearchResultProjectDto
    {
        public string Id { get; set; }
        public string ProjectTitle { get; set; }
        public string WorkingEmail { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public String OwnerId { get; set; }
        public Client.Dtos.ClientDto Owner { get; set; }
    }
}
