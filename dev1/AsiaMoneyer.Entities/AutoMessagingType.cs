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

    [Table("AutoMessagingTypes")]
    public class AutoMessagingType : Entity<int>
    {
        public AutoMessagingType()
        {
            this.AutoMessagingTemplates = new HashSet<AutoMessagingTemplate>();
        }
        
        public string MessagingTypeTitle { get; set; }
        public bool IsEnable { get; set; }

        public virtual ICollection<AutoMessagingTemplate> AutoMessagingTemplates { get; set; }
    }
}
