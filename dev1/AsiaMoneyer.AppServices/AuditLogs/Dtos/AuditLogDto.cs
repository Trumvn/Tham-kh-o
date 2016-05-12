using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.AuditLog.Dtos
{
    public class AuditLogDto
    {
        public string Id { get; set; }
        public string UserID { get; set; }
        public DateTime EventDateUTC { get; set; }
        public string EventType { get; set; }
        public string TableName { get; set; }
        public string RecordID { get; set; }
        public string ColumnName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }

        public Client.Dtos.UserDto User { get; set; }

        public string Title { get; set; }
        public string EventText { get; set; }
        public string CustomText { get; set; }
    }
}
