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

    [Table("UserComments")]
    public partial class UserComment : Entity<string>, Base.IAuditable
    {
        public int AppObjectTypeId { get; set; }
        public string UserId { get; set; }
        public string ObjectId { get; set; }
        public System.DateTime CommentDate { get; set; }
        public string CommentText { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("AppObjectTypeId")]
        public virtual AppObjectType AppObjectType { get; set; }

    }
}
