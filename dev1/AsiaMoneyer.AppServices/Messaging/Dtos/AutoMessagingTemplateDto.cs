using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging.Dtos
{
    public class AutoMessagingTemplateDto
    {
        public string Id { get; set; }
        public int AutoMessagingTypeId { get; set; }
        public int AutoMessagingSenderId { get; set; }
        public string MessagingTemplateName { get; set; }
        public bool IsPublish { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
