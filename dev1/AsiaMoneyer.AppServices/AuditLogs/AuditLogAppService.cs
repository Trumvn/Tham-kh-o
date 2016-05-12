using AsiaMoneyer.Project.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.AuditLog
{
    public class AuditLogAppService : AppServiceBase, IAuditLogAppService
    {
        #region Private Methods
        private void ParserAuditLogText(Dtos.AuditLogDto log)
        {
            log.CustomText = log.NewValue;
            string oldValueText = String.Empty;

            // Value
            if (String.Compare(log.ColumnName, "IsClear", true) == 0)
            {
                log.CustomText = String.Compare(log.NewValue, "true", true) == 0 ? "done" : "un-done";
            }
            else if (String.Compare(log.ColumnName, "TransactionDate", true) == 0)
            {
                if (!String.IsNullOrEmpty(log.NewValue))
                {
                    DateTime date = DateTime.Parse(log.NewValue);
                    log.CustomText = date.ToString("dd/MM/yyyy");
                }
                else
                {
                    if (!String.IsNullOrEmpty(log.OriginalValue))
                    {
                        DateTime date = DateTime.Parse(log.OriginalValue);
                        oldValueText = date.ToString("dd/MM/yyyy");
                    }
                }
            }
            else if (String.Compare(log.ColumnName, "Id", true) == 0)
            {
                log.CustomText = String.Empty;
            }
            else if (String.Compare(log.ColumnName, "AccountId", true) == 0)
            {
                log.CustomText = String.Empty;
                Entities.Account account = this.UnitOfWork.AccountRepository.Get(log.NewValue);
                if (account != null)
                {
                    log.CustomText = account.AccountTitle;
                }
            }
            else if (String.Compare(log.ColumnName, "CategoryId", true) == 0)
            {
                log.CustomText = String.Empty;

                if (!String.IsNullOrEmpty(log.NewValue))
                {
                    Entities.Category category = this.UnitOfWork.CategoryRepository.Get(log.NewValue);
                    if (category != null)
                    {
                        log.CustomText = category.CategoryTitle;
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(log.OriginalValue))
                    {
                        Entities.Category category = this.UnitOfWork.CategoryRepository.Get(log.OriginalValue);
                        if (category != null)
                        {
                            oldValueText = category.CategoryTitle;
                        }
                    }

                }
            }
            else if (String.Compare(log.ColumnName, "ClientId", true) == 0)
            {
                log.CustomText = String.Empty;
                if (!String.IsNullOrEmpty(log.NewValue))
                {
                    Entities.Client client = this.UnitOfWork.ClientRepository.Get(log.NewValue);
                    if (client != null)
                    {
                        log.CustomText = String.Format("{0} {1}", client.FirstName, client.LastName);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(log.OriginalValue))
                    {
                        Entities.Client client = this.UnitOfWork.ClientRepository.Get(log.OriginalValue);
                        if (client != null)
                        {
                            oldValueText = String.Format("{0} {1}", client.FirstName, client.LastName);
                        }
                    }

                }
            }
            else if (String.Compare(log.ColumnName, "RecurringTransactionId", true) == 0)
            {
                log.CustomText = String.Empty;
                if (!String.IsNullOrEmpty(log.NewValue))
                {
                    Entities.RecurringTransaction recurringTransaction = this.UnitOfWork.RecurringTransactionRepository.Get(log.NewValue);
                    if (recurringTransaction != null)
                    {
                        log.CustomText = String.Format("{0}", recurringTransaction.TimeFrequency.TimeFrequencyTitle);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(log.OriginalValue))
                    {
                        Entities.RecurringTransaction recurringTransaction = this.UnitOfWork.RecurringTransactionRepository.Get(log.OriginalValue);
                        if (recurringTransaction != null)
                        {
                            oldValueText = String.Format("{0}", recurringTransaction.TimeFrequency.TimeFrequencyTitle);
                        }
                    }
                }
            }
            else if (String.Compare(log.ColumnName, "IsClosed", true) == 0)
            {
                log.CustomText = String.Compare(log.NewValue, "true", true) == 0 ? "Disable" : "Enable";
            }
            // Event
            if (String.Compare(log.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_ADD, true) == 0)
            {
                log.EventText = "created";
            }
            else if (String.Compare(log.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_MODIFY, true) == 0)
            {
                if (!String.IsNullOrEmpty(log.CustomText))
                {
                    log.EventText = "changed to";
                }
                else
                {
                    if (!String.IsNullOrEmpty(oldValueText))
                    {
                        log.EventText = "cleared";
                        log.CustomText = oldValueText;
                    }
                }
            } if (String.Compare(log.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_DELETE, true) == 0)
            {
                log.EventText = "deleted";
            }
        }

        private void ParserAuditLogTitle(Dtos.AuditLogDto log)
        {
            string title = String.Empty;
            if (String.Compare(log.TableName, Commons.Constants.SYS_TABLE_NAME_PROJECTS, true) == 0)
            {
                log.CustomText = "project";
            }
            else if (String.Compare(log.TableName, Commons.Constants.SYS_TABLE_NAME_PROJECT_TRANSACTIONS, true) == 0)
            {
                log.CustomText = "transaction";
            }
            else if (String.Compare(log.TableName, Commons.Constants.SYS_TABLE_NAME_PROJECT_ACCOUNTS, true) == 0)
            {
                log.CustomText = "account";
            }
            else if (String.Compare(log.TableName, Commons.Constants.SYS_TABLE_NAME_PROJECT_CATEGORIES, true) == 0)
            {
                log.CustomText = "category";
            }
            else if (String.Compare(log.TableName, Commons.Constants.SYS_TABLE_NAME_PROJECT_MEMBERS, true) == 0)
            {
                log.CustomText = "project member";
            }

            title = this.UnitOfWork.AuditLogRepository.getObjectTitle(log.RecordID, log.TableName, log.EventDateUTC);

            log.Title = title;

            // Event
            if (String.Compare(log.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_ADD, true) == 0)
            {
                log.EventText = "created";
            }
            else if (String.Compare(log.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_MODIFY, true) == 0)
            {
                log.EventText = "modified";
            } 
            else if (String.Compare(log.EventType, Commons.Constants.AUDIT_LOG_EVENT_TYPE_DELETE, true) == 0)
            {
                log.EventText = "deleted";
            }
        }
        #endregion

        #region Override Methods

        public List<Dtos.AuditLogDto> GetTransactionAuditLogs(string transactionId)
        {
            int top = Commons.Constants.APP_SETTING_AUDIT_LOG_QUERY_TOP_VALUE;
            List<Entities.AuditLog> auditLogEntities = this.UnitOfWork.AuditLogRepository.GetAuditLogs(transactionId, Commons.Constants.SYS_TABLE_NAME_PROJECT_TRANSACTIONS, top);
            List<Dtos.AuditLogDto> auditLogDtos = AutoMapper.Mapper.Map<List<Entities.AuditLog>, List<Dtos.AuditLogDto>>(auditLogEntities);

            foreach(Dtos.AuditLogDto log in auditLogDtos)
            {
                this.ParserAuditLogText(log);
            }

            return auditLogDtos;
        }

        public List<Dtos.AuditLogDto> GetAccountAuditLogs(string AccountId)
        {
            int top = Commons.Constants.APP_SETTING_AUDIT_LOG_QUERY_TOP_VALUE;
            List<Entities.AuditLog> auditLogEntities = this.UnitOfWork.AuditLogRepository.GetAuditLogs(AccountId, Commons.Constants.SYS_TABLE_NAME_PROJECT_ACCOUNTS, top);
            List<Dtos.AuditLogDto> auditLogDtos = AutoMapper.Mapper.Map<List<Entities.AuditLog>, List<Dtos.AuditLogDto>>(auditLogEntities);

            foreach (Dtos.AuditLogDto log in auditLogDtos)
            {
                this.ParserAuditLogText(log);
            }

            return auditLogDtos;
        }
        public List<Dtos.AuditLogDto> GetCategoryAuditLogs(string CategoryId)
        {
            int top = Commons.Constants.APP_SETTING_AUDIT_LOG_QUERY_TOP_VALUE;
            List<Entities.AuditLog> auditLogEntities = this.UnitOfWork.AuditLogRepository.GetAuditLogs(CategoryId, Commons.Constants.SYS_TABLE_NAME_PROJECT_CATEGORIES, top);
            List<Dtos.AuditLogDto> auditLogDtos = AutoMapper.Mapper.Map<List<Entities.AuditLog>, List<Dtos.AuditLogDto>>(auditLogEntities);

            foreach (Dtos.AuditLogDto log in auditLogDtos)
            {
                this.ParserAuditLogText(log);
            }

            return auditLogDtos;
        }

        public List<Dtos.AuditLogDto> GetAuditLogs(string objId, string tableName)
        {
            int top = Commons.Constants.APP_SETTING_AUDIT_LOG_QUERY_TOP_VALUE;
            List<Entities.AuditLog> auditLogEntities = this.UnitOfWork.AuditLogRepository.GetAuditLogs(objId, tableName, top);

            List<Dtos.AuditLogDto> auditLogDtos = AutoMapper.Mapper.Map<List<Entities.AuditLog>, List<Dtos.AuditLogDto>>(auditLogEntities);
            return auditLogDtos;
        }

        public void AddProjectAuditLog(int projectId, string logText)
        {
            //this.AddAuditLog(Commons.Constants.APP_OBJECT_TYPE_ID_PROJECT_LOGS, projectId, logText);
        }

        public void AddTransactionAuditLog(int transactionId, string logText)
        {
            //this.AddAuditLog(Commons.Constants.APP_OBJECT_TYPE_ID_TRANSACTION_LOGS, transactionId, logText);
        }

        public List<Dtos.AuditLogDto> GetProjectAuditLogs(string projectId, PagingDto paging)
        {
            List<Entities.AuditLog> auditLogEntities = this.UnitOfWork.AuditLogRepository.GetProjectAuditLogs(projectId, paging.Top, paging.Page);
            List<Dtos.AuditLogDto> auditLogDtos = AutoMapper.Mapper.Map<List<Entities.AuditLog>, List<Dtos.AuditLogDto>>(auditLogEntities);

            foreach (Dtos.AuditLogDto log in auditLogDtos)
            {
                this.ParserAuditLogTitle(log);
            }

            return auditLogDtos;
        }
        #endregion
    }
}
