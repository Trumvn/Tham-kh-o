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

    [Table("AppObjectTypes")]
    public partial class AppObjectType : Entity<int>
    {
        public AppObjectType()
        {
            this.UserComments = new HashSet<UserComment>();
        }

        public string AppObjectTypeTitle { get; set; }

        public virtual ICollection<UserComment> UserComments { get; set; }
    }
}
