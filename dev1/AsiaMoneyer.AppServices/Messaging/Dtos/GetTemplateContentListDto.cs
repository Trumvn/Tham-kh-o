using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging.Dtos
{
    public class GetTemplateContentListDto
    {
        public String TemplateId { get; set; }
        public String TemplateName { get; set; }
        public int Total { get; set; }
        public List<TemplateContentDto> TemplateContentList { get; set; }
    }
}
