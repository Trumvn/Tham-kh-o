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

    [Table("AutoMessagingDataMapping")]
    public class AutoMessagingDataMapping : Entity<string>
    {
        public AutoMessagingDataMapping()
        {

        }


        public string MappingName { get; set; }
        public string TokenKey { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string RequiredColumnName { get; set; }
        public string Format { get; set; }
        public string SqlQuery { get; set; }
        public string Value { get; set; }
        public bool IsPublish { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
