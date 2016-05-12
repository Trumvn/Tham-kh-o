using AsiaMoneyer.Project.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.AuditLog
{
    public interface IAuditLogAppService : IAppService
    {
        List<Dtos.AuditLogDto> GetAuditLogs(string objId, string tableName);
        List<Dtos.AuditLogDto> GetTransactionAuditLogs(string transactionId);
        List<Dtos.AuditLogDto> GetAccountAuditLogs(string AccountId);
        List<Dtos.AuditLogDto> GetCategoryAuditLogs(string CategoryId);

        List<Dtos.AuditLogDto> GetProjectAuditLogs(string projectId, PagingDto paging);
    }
}
