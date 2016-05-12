using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Entities
{
    public class AutoMessagingMessageModel
    {
        public AutoMessagingMessageModel()
        {
            Checked = false;
        }

        public string Id { get; set; }
        public string AutoMessagingTemplateContentId { get; set; }
        public int AutoMessagingSenderId { get; set; }
        public string MessagingSubject { get; set; }
        public string MessagingFromName { get; set; }
        public string MessagingFromEmailAddress { get; set; }
        public string MessagingTo { get; set; }
        public string MessagingCc { get; set; }
        public string MessagingBcc { get; set; }
        public string MessagingContent { get; set; }
        public string Tags { get; set; }
        public bool IsSent { get; set; }
        public bool IsMarkedAsRead { get; set; }
        public Nullable<DateTime> SentDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public string TemplateName { get; set; }
        public bool Checked { get; set; }
    }
}
