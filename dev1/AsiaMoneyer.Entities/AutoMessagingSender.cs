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

    [Table("AutoMessagingSenders")]
    public class AutoMessagingSender : Entity<int>
    {
        public AutoMessagingSender()
        {
            this.AutoMessagingTemplates = new HashSet<AutoMessagingTemplate>();
        }

        public string MessagingSenderTitle { get; set; }
        public string ProviderName { get; set; }
        public string ProviderHost { get; set; }
        public int ProviderPort { get; set; }
        public bool ProviderEnableSsl { get; set; }
        public string CredentialUserName { get; set; }
        public string DisplayName { get; set; }
        public string CredentialPasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool IsEnable { get; set; }

        public virtual ICollection<AutoMessagingTemplate> AutoMessagingTemplates { get; set; }
    }
}
