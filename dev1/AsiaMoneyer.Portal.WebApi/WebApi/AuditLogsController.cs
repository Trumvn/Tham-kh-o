using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AsiaMoneyer.AuditLog;

namespace AsiaMoneyer.Portal.WebApi.Controllers
{
    public class AuditLogsController : BaseApiController
    {
        IAuditLogAppService _auditLogAppService;

        public AuditLogsController(IAuditLogAppService auditLogAppService)
        {
            this._auditLogAppService = auditLogAppService;
        }

        #region AuditLogs
        public IHttpActionResult GetTransactionAuditLogs(string transactionId)
        {
            var l = this._auditLogAppService.GetTransactionAuditLogs(transactionId);
            return Ok(l);
        }

        public IHttpActionResult GetProjectAccountAuditLogs(string accountId)
        {
            //string tableName = Commons.Constants.SYS_TABLE_NAME_PROJECT_ACCOUNTS;
            //var l = this._auditLogAppService.GetAuditLogs(accountId, tableName);
            var l = this._auditLogAppService.GetAccountAuditLogs(accountId);
            return Ok(l);
        }

        public IHttpActionResult GetProjectCategoryAuditLogs(string categoryId)
        {
            //string tableName = Commons.Constants.SYS_TABLE_NAME_PROJECT_CATEGORIES;
            //var l = this._auditLogAppService.GetAuditLogs(categoryId, tableName);
            var l = this._auditLogAppService.GetCategoryAuditLogs(categoryId);
            return Ok(l);
        }

        #endregion
    }
}