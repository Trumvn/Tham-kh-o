using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AutoMessagingMessages")]
    public class AutoMessagingMessage : Entity<string>
    {
        public AutoMessagingMessage()
        {

        }

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

        [ForeignKey("AutoMessagingTemplateContentId")]
        public virtual AutoMessagingTemplateContent AutoMessagingTemplateContent { get; set; }

        [ForeignKey("AutoMessagingSenderId")]
        public virtual AutoMessagingSender AutoMessagingSender { get; set; }

    }
}
