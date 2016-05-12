using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.AuditLog;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class AuditLogRepository : AsiaMoneyerRepositoryBase<Entities.AuditLog, Guid>, IAuditLogRepository
    {
        public AuditLogRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        #region Override Methods
        public List<AsiaMoneyer.Entities.AuditLog> GetAuditLogs(string objId, string tableName, int top)
        {
            List<Entities.AuditLog> auditLogs = this.Get(x => String.Compare(x.RecordID, objId, true) == 0 && String.Compare(x.TableName, tableName, true) == 0
                && (String.Compare(x.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_MODIFY, true) == 0 || (String.Compare(x.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_ADD, true) == 0 && String.Compare(x.ColumnName, "Id", true) == 0)))
                .OrderByDescending(o => o.EventDateUTC).Take(top).ToList();
            return auditLogs;
        }

        public List<AsiaMoneyer.Entities.AuditLog> GetProjectAuditLogs(string projectId, int pageSize, int pageIndex)
        {
            List<String> tableBelongToProjects = new List<string>() { Commons.Constants.SYS_TABLE_NAME_PROJECTS, Commons.Constants.SYS_TABLE_NAME_PROJECT_TRANSACTIONS, Commons.Constants.SYS_TABLE_NAME_PROJECT_ACCOUNTS, Commons.Constants.SYS_TABLE_NAME_PROJECT_CATEGORIES, Commons.Constants.SYS_TABLE_NAME_PROJECT_MEMBERS };

            var projectCategories = this.DbContext.CategoryDbSet.Where(x => String.Compare(x.ProjectId, projectId, true) == 0).Select(x=>x.Id);
            var projectAccounts = this.DbContext.AccountDbSet.Where(x => String.Compare(x.ProjectId, projectId, true) == 0).Select(x => x.Id);
            var projectMembers = this.DbContext.ProjectMemberDbSet.Where(x => String.Compare(x.ProjectId, projectId, true) == 0).Select(x => x.Id);
            var projectTransactions = this.DbContext.TransactionDbSet.Where(x => String.Compare(x.ProjectId, projectId, true) == 0).Select(x => x.Id);

            List<Entities.AuditLog> auditLogs = this.Get(x => tableBelongToProjects.Contains(x.TableName) && (String.Compare(x.RecordID, projectId, true) == 0 || projectCategories.Contains(x.RecordID) || projectAccounts.Contains(x.RecordID) || projectMembers.Contains(x.RecordID) || projectTransactions.Contains(x.RecordID))
                && (String.Compare(x.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_MODIFY, true) == 0 || (String.Compare(x.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_ADD, true) == 0 && String.Compare(x.ColumnName, "Id", true) == 0)))
                .OrderByDescending(o => o.EventDateUTC).Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return auditLogs;
        }

        public string getObjectTitle(string objectId, string tableName, DateTime auditLogDate)
        {
            string title = String.Empty;

            title = this.List.Where(x => String.Compare(x.TableName, tableName, true) == 0 && x.ColumnName.ToLower().Contains("title") && String.Compare(x.RecordID, objectId, true) == 0 && x.EventDateUTC <= auditLogDate).OrderByDescending(x => x.EventDateUTC).Select(x => x.NewValue).FirstOrDefault();

            return title;
        }
        #endregion
    }
}
