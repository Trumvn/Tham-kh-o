using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class PagingDto
    {
        public string Id { get; set; }
        public int Page { get; set; }
        public int Top { get; set; }
    }
}
