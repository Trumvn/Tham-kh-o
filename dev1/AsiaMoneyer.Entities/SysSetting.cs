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

    [Table("SysSettings")]
    public partial class SysSetting : Entity<int>
    {
        [Required]
        public string Key { get; set; }
        public string Lang { get; set; }
        public string Value { get; set; }
    }
}
