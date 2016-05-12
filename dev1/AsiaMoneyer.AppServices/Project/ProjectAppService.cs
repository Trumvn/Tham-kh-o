using AsiaMoneyer.Account;
using AsiaMoneyer.Client.Dtos;
using AsiaMoneyer.Category;
using AsiaMoneyer.EventBus;
using AsiaMoneyer.Project.Dtos;
using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.AuditLog;
using AsiaMoneyer.AuditLog.Dtos;
using AsiaMoneyer.Messaging;
using AsiaMoneyer.SystemSettings;
using AsiaMoneyer.Commons;

namespace AsiaMoneyer.Project
{
    public class ProjectAppService: AppServiceBase, IProjectAppService
    {        
        private readonly IAutoMessagingAppService _autoMessagingAppService;

        private readonly IAuditLogAppService _auditLogAppService;

        private readonly ISysSettingAppService _sysSettingAppService;

        private readonly IEventPublisher _eventPublisher;

        public ProjectAppService(IAuditLogAppService auditLogAppService, IEventPublisher eventPublisher, IAutoMessagingAppService autoMessagingAppService, ISysSettingAppService sysSettingAppService)
        {
            this._auditLogAppService = auditLogAppService;

            this._eventPublisher = eventPublisher;

            this._autoMessagingAppService = autoMessagingAppService;

            this._sysSettingAppService = sysSettingAppService;
        }

        #region Project
        public Dtos.ProjectHeaderDto GetProjectHeader(string projectId)
        {
            Entities.Project projectEntity = this.UnitOfWork.ProjectRepository.Get(projectId);
            Dtos.ProjectHeaderDto projectHeaderDto = AutoMapper.Mapper.Map<Entities.Project, ProjectHeaderDto>(projectEntity);
            projectHeaderDto.CurrentBalance = this.UnitOfWork.TransactionRepository.GetBalance(projectId, DateTime.Now);
            return projectHeaderDto;
        }

        public Dtos.GetProjectsOutput GetProjects()
        {
            Dtos.GetProjectsOutput projects = new Dtos.GetProjectsOutput();

            List<AsiaMoneyer.Entities.Project> projectEntities = this.UnitOfWork.ProjectRepository.GetProjects();
            projects.Projects = AutoMapper.Mapper.Map<List<Entities.Project>, List<Project.Dtos.ProjectDto>>(projectEntities);

            //_eventPublisher.Publish(new OrderSubmittedEvent { OrderId = 59 });

            return projects;
        }

        public Dtos.GetProjectsOutput GetProjects(Project.Dtos.ProjectFilterDto filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Project Filter cannot be null");

            Dtos.GetProjectsOutput projects = new Dtos.GetProjectsOutput();

            List<AsiaMoneyer.Entities.Project> projectEntities = null;

            switch(filter.FilterBy)
            {
                case Constants.PROJECT_FILTER.ALL:
                    break;
                case Constants.PROJECT_FILTER.LIST:
                    projectEntities = this.UnitOfWork.ProjectRepository.GetUserProjects(filter.UserId);
                    break;
                case Constants.PROJECT_FILTER.SHARE:
                    projectEntities = this.UnitOfWork.ProjectRepository.GetUserSharedProjects(filter.UserId);
                    break;
                case Constants.PROJECT_FILTER.ARCHIVED:
                    projectEntities = this.UnitOfWork.ProjectRepository.GetAllArchivedProjects(filter.UserId);
                    break;
            }            

            if (projectEntities != null)
            {
                projects.Projects = AutoMapper.Mapper.Map<List<Entities.Project>, List<Project.Dtos.ProjectDto>>(projectEntities);

                DateTime transactionSummaryDateFrom = DateTime.MinValue;
                DateTime transactionSummaryDateTo = DateTime.Now;

                DateTime startMonth = Commons.Ultility.GetFirstDateOfMonth(DateTime.Now);
                DateTime endMonth = Commons.Ultility.GetLastDateOfMonth(DateTime.Now); ;

                foreach (Project.Dtos.ProjectDto project in projects.Projects)
                {
                    Entities.Client client = this.UnitOfWork.ProjectRepository.GetProjectOwner(project.Id);
                    project.Owner = AutoMapper.Mapper.Map<Entities.Client, Client.Dtos.ClientDto>(client);

                    Entities.ProjectMember member = this.UnitOfWork.ProjectRepository.GetProjectMember(project.Id, filter.UserId);
                    project.User = AutoMapper.Mapper.Map<Entities.ProjectMember, Dtos.ProjectMemberDto>(member);

                    // Get Unclear Transactions
                    TransactionSumModel transactionSum = this.UnitOfWork.TransactionRepository.GetUnclearTransactionBalances(project.Id, transactionSummaryDateFrom, transactionSummaryDateTo, null);
                    project.TransactionSummary = AutoMapper.Mapper.Map<TransactionSumModel, TransactionSumDto>(transactionSum);

                    // Calculate Budget Summary
                    BudgetSumModel budgetExpenseSum = this.UnitOfWork.TransactionRepository.GetBudgetSummaryByMonth(project.Id, startMonth, endMonth, false);
                    project.BudgetExpenseSummary = AutoMapper.Mapper.Map<BudgetSumModel, BudgetSumDto>(budgetExpenseSum);

                    BudgetSumModel budgetIncomeSum = this.UnitOfWork.TransactionRepository.GetBudgetSummaryByMonth(project.Id, startMonth, endMonth, true);
                    project.BudgetIncomeSummary = AutoMapper.Mapper.Map<BudgetSumModel, BudgetSumDto>(budgetIncomeSum);

                }                
            }
            return projects;
        }

        public Dtos.GetProjectsOutput FilterProjects(Dtos.ProjectFilterDto filter)
        {
            Dtos.GetProjectsOutput projectOutput = new Dtos.GetProjectsOutput();
            projectOutput.Filter = filter;

            List<AsiaMoneyer.Entities.Project> projectEntities = this.UnitOfWork.ProjectRepository.GetProjects();
            projectOutput.Projects = AutoMapper.Mapper.Map<List<Entities.Project>, List<Project.Dtos.ProjectDto>>(projectEntities);

            return projectOutput;
        }

        public Dtos.ProjectSearchOutput SearchProjects(Dtos.ProjectFilterDto filter)
        {
            Dtos.ProjectSearchOutput searchResult = new Dtos.ProjectSearchOutput();
            searchResult.Filter = filter;

            List<AsiaMoneyer.Entities.Project> projects = this.UnitOfWork.ProjectRepository.GetProjects();
            searchResult.Projects = AutoMapper.Mapper.Map<List<Entities.Project>, List<Project.Dtos.SearchResultProjectDto>>(projects);

            foreach(Project.Dtos.SearchResultProjectDto project in searchResult.Projects)
            {
                Entities.Client client = this.UnitOfWork.ProjectRepository.GetProjectOwner(project.Id);
                project.Owner = AutoMapper.Mapper.Map<Entities.Client, Client.Dtos.ClientDto>(client);
                project.OwnerId = project.Owner != null ? project.Owner.Id : null;
            }

            return searchResult;
        }

        public Dtos.ProjectDto GetProject(string projectId)
        {
            List<AsiaMoneyer.Entities.Project> projectEntities = this.UnitOfWork.ProjectRepository.GetProjects();
            AsiaMoneyer.Entities.Project proj = projectEntities.Where(x => x.Id == projectId).FirstOrDefault();

            Dtos.ProjectDto projectDto = AutoMapper.Mapper.Map<AsiaMoneyer.Entities.Project, Dtos.ProjectDto>(proj);

            return projectDto;
        }

        public Dtos.ProjectSummaryDto GetProjectSummary(string projectId)
        {
            AsiaMoneyer.Entities.Project proj = this.UnitOfWork.ProjectRepository.Get(projectId);

            Dtos.ProjectSummaryDto projectsummaryDto=new ProjectSummaryDto();
            projectsummaryDto.Project = AutoMapper.Mapper.Map<AsiaMoneyer.Entities.Project, Dtos.ProjectHeaderDto>(proj);

            AsiaMoneyer.Entities.Client owner = this.UnitOfWork.ProjectRepository.GetProjectOwner(projectId);
            projectsummaryDto.Owner = AutoMapper.Mapper.Map<AsiaMoneyer.Entities.Client, ClientDto>(owner);

            return projectsummaryDto;
        }

        private int AddAccounts(Entities.Project project, string accounts)
        {
            int rowCount = 0;
            if (project != null)
            {
                string[] accountList = accounts.Split(',');

                foreach(string accountName in accountList)
                {
                    if(!String.IsNullOrEmpty(accountName))
                    {
                        Entities.Account account = new Entities.Account()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Project = project,
                            AccountTitle = Commons.Ultility.NormalizeSqlString(accountName),
                            IsClosed = false,
                            IsPrimary = (rowCount == 0? true : false),
                            IsDeleted = false,
                            OpenDate = DateTime.Now,
                            CreatedDate = DateTime.Now
                        };
                        this.UnitOfWork.AccountRepository.Add(account);
                        rowCount++;
                    }
                }
            }
            return rowCount;
        }

        private int AddCategories(Entities.Project project, string categories, bool isIncome)
        {
            int rowCount = 0;
            if (project != null)
            {
                string[] categoryList = categories.Split(',');

                foreach (string categoryName in categoryList)
                {
                    if (!String.IsNullOrEmpty(categoryName))
                    {
                        Entities.Category category = new Entities.Category()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Project = project,
                            CategoryTitle = Commons.Ultility.NormalizeSqlString(categoryName),
                            IsIncome = isIncome,
                            IsClosed = false,
                            ClosedDate = null,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };
                        this.UnitOfWork.CategoryRepository.Add(category);
                        rowCount++;
                    }
                }
            }
            return rowCount;
        }

        public void CreateProject(Dtos.ProjectDto projectDto)
        {
            Entities.Client userProfile = this.UnitOfWork.ClientRepository.List.Where(x => x.UserId == this.UserId).FirstOrDefault();

            var proj = new Entities.Project()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectTitle = Commons.Ultility.NormalizeSqlString(projectDto.ProjectTitle),
                ProjectDesc = Commons.Ultility.NormalizeSqlString(projectDto.ProjectDesc),
                Currency = projectDto.Currency,
                FinanceYearStartMonth = projectDto.FinanceYearStartMonth,
                FinanceYearMonths = Commons.Constants.PROJECT_DEFAULT_FINANCE_YEAR_MONTHS,
                IsPrivate = projectDto.IsPrivate,                
                IsDeleted = false,
                CreatedDate = DateTime.Now,                
            };

            var pu = new ProjectMember()
            {
                Id = Guid.NewGuid().ToString(),
                Project = proj,
                Client = userProfile,
                IsArchived = false,
                IsFollowing = true,
                IsFavorite = false,
                PermissionId = (int)Commons.Constants.PROJECT_PERMISSION.OWNER
            };

            userProfile.ProjectMembers.Add(pu);

            String defaultLanguge = "en";
            String defaultAccounts = this._sysSettingAppService.GetSysSetting(Commons.Constants.SYS_SETTING_DEFAULT_PROJECT_ACCOUNT_KEY, defaultLanguge);
            String defaultIncomeCategories = this._sysSettingAppService.GetSysSetting(Commons.Constants.SYS_SETTING_DEFAULT_PROJECT_INCOME_CATEGORY_KEY, defaultLanguge);
            String defaultExpenseCategories = this._sysSettingAppService.GetSysSetting(Commons.Constants.SYS_SETTING_DEFAULT_PROJECT_EXPENSE_CATEGORY_KEY, defaultLanguge);

            this.AddAccounts(proj, defaultAccounts);
            this.AddCategories(proj, defaultExpenseCategories, false);
            this.AddCategories(proj, defaultIncomeCategories, true);

            this.UnitOfWork.Save(this.UserId);
        }

        public void SaveProject(Dtos.ProjectDto projectDto)
        {
            var proj = new Entities.Project()
            {
                Id = projectDto.Id,
                ProjectTitle = Commons.Ultility.NormalizeSqlString(projectDto.ProjectTitle),
                ProjectDesc = Commons.Ultility.NormalizeSqlString(projectDto.ProjectDesc),
                Currency = projectDto.Currency,
                FinanceYearStartMonth = projectDto.FinanceYearStartMonth,
                IsPrivate = projectDto.IsPrivate
            };

            this.UnitOfWork.ProjectRepository.Update(proj, u => u.ProjectTitle, u => u.ProjectDesc, u => u.Currency, u => u.FinanceYearStartMonth, u => u.IsPrivate);

            this.UnitOfWork.Save(this.UserId);
        }

        public void DeleteProject(string projectId)
        {
            var proj = new Entities.Project()
            {
                Id = projectId,
                IsDeleted = true
            };

            this.UnitOfWork.ProjectRepository.Update(proj, u => u.IsDeleted);

            this.UnitOfWork.Save(this.UserId);
        }

        public void SetProjectHighlightColor(Dtos.ProjectDto projectDto)
        {
            Entities.ProjectMember member = this.UnitOfWork.ProjectRepository.GetProjectMember(projectDto.Id, this.UserId);

            member.HighlightColor = projectDto.HighlightColor;

            this.UnitOfWork.Save(this.UserId);

        }

        public void SetProjectArchived(Dtos.ProjectDto projectDto)
        {
            Entities.ProjectMember member = this.UnitOfWork.ProjectRepository.GetProjectMember(projectDto.Id, this.UserId);

            member.IsArchived = projectDto.IsArchived??false;

            this.UnitOfWork.Save(this.UserId);

        }

        public void SetProjectFavorite(Dtos.ProjectDto projectDto)
        {
            Entities.ProjectMember member = this.UnitOfWork.ProjectRepository.GetProjectMember(projectDto.Id, this.UserId);

            member.IsFavorite = projectDto.IsFavorite ?? false;

            this.UnitOfWork.Save(this.UserId);

        }

        public List<AuditLogDto> GetProjectRecentActivity(string projectId, PagingDto paging)
        {
            List<AuditLogDto> projectAuditLogs = _auditLogAppService.GetProjectAuditLogs(projectId, paging);

            return projectAuditLogs;
        }

        #endregion

        #region Project Member
        public List<Dtos.ProjectMemberDto> InviteMember(string projectId, string memberEmail)
        {
            if (String.IsNullOrEmpty(projectId))
                throw new ArgumentNullException("ProjectId cannot be null");

            if (String.IsNullOrEmpty(memberEmail))
                throw new ArgumentNullException("Member email cannot be null");

            memberEmail = memberEmail.ToLower();

            Entities.Client userProfile = this.UnitOfWork.ClientRepository.List.Where(x => x.EmailAddress == memberEmail).FirstOrDefault();

            if(userProfile == null)
            {
                userProfile = new Entities.Client
                {
                    Id = Guid.NewGuid().ToString(),
                    EmailAddress = memberEmail,
                    FirstName = memberEmail,
                };
            }

            Entities.Project project = this.UnitOfWork.ProjectRepository.Get(projectId);

            var pu = new ProjectMember()
            {
                Id = Guid.NewGuid().ToString(),
                Project = project,
                Client = userProfile,
                IsArchived = false,
                IsFollowing = true,
                IsFavorite = false,
                PermissionId = (int) Commons.Constants.PROJECT_PERMISSION.MEMBER
            };

            project.ProjectMembers.Add(pu);

            this.UnitOfWork.Save(this.UserId);

            return GetProjectMembers(projectId);
        }

        public List<Dtos.ProjectMemberDto> RemoveMember(string projectMemberId)
        {
            string projectId = this.UnitOfWork.ProjectRepository.RemoveMember(projectMemberId);
            this.UnitOfWork.Save(this.UserId);

            return GetProjectMembers(projectId);

        }
        public void AcceptInvitition(string code)
        {
            Entities.Client userProfile = this.UnitOfWork.ClientRepository.List.Where(x => x.Id == code).FirstOrDefault();
            if( userProfile != null)
            {
                userProfile.UserId = this.UserId;
            }
            this.UnitOfWork.Save(this.UserId);
        }

        public List<Dtos.ProjectMemberDto> GetProjectMembers(string projectId)
        {
            List<Dtos.ProjectMemberDto> projectMembers = null;
            Entities.Project project = this.UnitOfWork.ProjectRepository.Get(projectId);
            if (project != null)
            {
                projectMembers = AutoMapper.Mapper.Map<List<Entities.ProjectMember>, List<Project.Dtos.ProjectMemberDto>>(project.ProjectMembers.ToList());
            }
            return projectMembers;
        }


        public void SaveProjectFilter(string projectId, int filter, string categoryId, string accountId, Nullable<DateTime> fromDate, Nullable<DateTime> endDate)
        {
            this.UnitOfWork.ProjectRepository.SaveProjectFilter(projectId, this.UserId, filter, categoryId, accountId, fromDate, endDate);
            this.UnitOfWork.Save(this.UserId);
        }

        public Dtos.TransactionFilterDto GetProjectFilter(string projectId)
        {
            Entities.ProjectMember member = this.UnitOfWork.ProjectRepository.GetProjectMember(projectId, this.UserId);

            AccountDto account = null;
            if (!String.IsNullOrEmpty(member.ViewFilterByAccount))
            {
                account = new AccountDto()
                {
                    Id = member.ViewFilterByAccount
                };
            }

            CategoryDto category = null;
            if (!String.IsNullOrEmpty(member.ViewFilterByCategory))
            {
                category = new CategoryDto()
                {
                    Id = member.ViewFilterByCategory
                };
            }

            Dtos.TransactionFilterDto filter = new TransactionFilterDto()
            {
                ProjectId = projectId,
                FilterTypeId = (member.ViewFilterByType != null ? (Commons.Constants.TRANSACTION_FILTER)member.ViewFilterByType : 0),
                Account = account,
                Category = category,
                FromDate = member.ViewFilterFromDate,
                EndDate = member.ViewFilterEndDate
            };


            if (!filter.FromDate.HasValue || this.IsCustomFilterTime(filter.FilterTypeId))
            {
                DateTime queryDate = DateTime.Today;
                TimeRange timeRange = DatetimeHelper.GetTimeRange(filter.FilterTypeId, queryDate);
                filter.FromDate = timeRange.From;
                filter.EndDate = timeRange.End;
                filter.FilterTypeId = Constants.TRANSACTION_FILTER.TODAY;
            }
            

            return filter;
        }

        private bool IsCustomFilterTime(Commons.Constants.TRANSACTION_FILTER filterType)
        {
            return (filterType == Commons.Constants.TRANSACTION_FILTER.CUSTOM_DATE) || (filterType == Commons.Constants.TRANSACTION_FILTER.CUSTOM_MONTH) || (filterType == Commons.Constants.TRANSACTION_FILTER.CUSTOM_WEEK) || (filterType == Commons.Constants.TRANSACTION_FILTER.CUSTOM_YEAR);
        }

        public List<Dtos.ProjectHeaderDto> GetFavoriteProjectHeaders(string userId)
        {
            List<AsiaMoneyer.Entities.Project> projectEntities = null;
            projectEntities = this.UnitOfWork.ProjectRepository.GetFavoriteProjects(userId);
            List<Dtos.ProjectHeaderDto> projects = AutoMapper.Mapper.Map<List<Entities.Project>, List<Project.Dtos.ProjectHeaderDto>>(projectEntities);
            return projects;
        }
        #endregion

        #region Private Methods
        private Client.Dtos.ClientDto GetProjectOnwer(string projectId)
        {
            return null;
        }
        #endregion
    }
}
