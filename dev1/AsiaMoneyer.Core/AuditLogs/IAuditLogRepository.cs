using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.AuditLog
{
    public interface IAuditLogRepository : IRepository<AsiaMoneyer.Entities.AuditLog, Guid>
    {
        List<AsiaMoneyer.Entities.AuditLog> GetAuditLogs(string objId, string tableName, int top);
        List<AsiaMoneyer.Entities.AuditLog> GetProjectAuditLogs(string projectId, int pageSize, int pageIndex);
        string getObjectTitle(string objectId, string tableName, DateTime auditLogDate);
    }
}
