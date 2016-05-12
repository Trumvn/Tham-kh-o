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

    [Table("AutoMessagingTemplates")]
    public class AutoMessagingTemplate : Entity<string>
    {
        public AutoMessagingTemplate()
        {
            
        }

        public int AutoMessagingTypeId { get; set; }
        public int AutoMessagingSenderId { get; set; }
        public string MessagingTemplateName { get; set; }
        public bool IsPublish { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("AutoMessagingTypeId")]
        public virtual AutoMessagingType AutoMessagingType { get; set; }

        [ForeignKey("AutoMessagingSenderId")]
        public virtual AutoMessagingSender AutoMessagingSender { get; set; }

    }
}
