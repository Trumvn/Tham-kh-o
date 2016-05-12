using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging.Dtos
{
    public class TemplateContentDto
    {
        public string Id { get; set; }
        public string AutoMessagingTemplateId { get; set; }
        public string AutoMessagingTemplateName { get; set; }
        public string Lang { get; set; }
        public string MessagingSubject { get; set; }
        public string MessagingFromName { get; set; }
        public string MessagingFromEmailAddress { get; set; }
        public string MessagingTo { get; set; }
        public string MessagingCc { get; set; }
        public string MessagingBcc { get; set; }
        public string MessagingContent { get; set; }
        public string Tags { get; set; }
        public bool IsPublish { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
