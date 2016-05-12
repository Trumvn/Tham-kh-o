using AsiaMoneyer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AsiaMoneyer.Project;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class ProjectRepository : AsiaMoneyerRepositoryBase<Entities.Project>, IProjectRepository
    {

        public ProjectRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
    
        }

        #region Override IRepository
        public List<Entities.Project> GetProjects()
        {
            List<Entities.Project> projects = this.List.ToList();
            return projects;
        }

        public List<Entities.Project> GetProjects(string userId, bool isIncludeArchived)
        {
            List<Entities.Project> projects = this.DbContext.ProjectMemberDbSet.Where(x => x.Client.UserId == userId && x.IsArchived == isIncludeArchived && x.Project.IsDeleted == false)
                .Select(x => x.Project)
                .ToList();

            return projects;
        }

        public List<AsiaMoneyer.Entities.Project> GetProjectTitleList(string userId)
        {
            List<Entities.Project> projects = this.DbContext.ProjectMemberDbSet.Where(x => x.Client.UserId == userId && x.Project.IsDeleted == false)
                .Select(x => x.Project)
                .ToList();

            /*List<Entities.Project> projects = this.DbContext.ProjectUserDbSet.Where(x => x.UserProfile.UserId == userId)
                .Select(x => new Entities.Project { Id = x.Project.Id, ProjectTitle = x.Project.ProjectTitle, IsArchive = x.Project.IsArchive, IsPrivate = x.Project.IsPrivate })
                .ToList();*/

            return projects;
        }

        public void RemoveMember(string projectId, string clientId)
        {
            Entities.ProjectMember projectMember = this.DbContext.ProjectMemberDbSet.Where(x => x.ClientId == clientId && x.ProjectId == projectId).FirstOrDefault();
            if (projectMember != null)
            {
                this.DbContext.ProjectMemberDbSet.Remove(projectMember);
            }
        }

        public string RemoveMember(string projectMemberId)
        {
            string projectId = String.Empty;
            Entities.ProjectMember projectMember = this.DbContext.ProjectMemberDbSet.Where(x => x.Id == projectMemberId).FirstOrDefault();
            if (projectMember != null)
            {
                projectId = projectMember.ProjectId;
                this.DbContext.ProjectMemberDbSet.Remove(projectMember);
            }

            return projectId;
        }

        public Entities.Client GetProjectOwner(string projectId)
        {
            Entities.ProjectMember projectMember = this.DbContext.ProjectMemberDbSet.Where(x => x.ProjectId == projectId && x.PermissionId == (int)Commons.Constants.PROJECT_PERMISSION.OWNER).FirstOrDefault();
            if (projectMember != null)
            {
                return projectMember.Client;
            }
            return null;
        }

        public Entities.ProjectMember GetProjectMember(string projectId, string userId)
        {
            Entities.ProjectMember projectMember = null;
            string clientId = this.DbContext.ClientDbSet.Where(x => x.UserId == userId).Select(x=>x.Id).FirstOrDefault();
            if (clientId != null)
            {
                projectMember = this.DbContext.ProjectMemberDbSet.Where(x => x.ProjectId == projectId && x.ClientId == clientId).FirstOrDefault();
            }

            return projectMember;
        }

        public string GetProjectCurrency(string projectId)
        {
            string currency = this.List.Where(x => x.Id == projectId).Select(x => x.Currency).FirstOrDefault();

            return currency;
        }

        public string GetProjectCurrencyByCategory(string categoryId)
        {
            string projectId = this.DbContext.CategoryDbSet.Where(x => x.Id == categoryId).Select(x => x.ProjectId).FirstOrDefault();

            return this.GetProjectCurrency(projectId);
        }

        public string GetProjectCurrencyByAccount(string accountId)
        {
            string projectId = this.DbContext.AccountDbSet.Where(x => x.Id == accountId).Select(x => x.ProjectId).FirstOrDefault();

            return this.GetProjectCurrency(projectId);
        }

        public int GetProjectFinanceYearStartMonth(string projectId)
        {
            int startMonth = this.List.Where(x => x.Id == projectId).Select(x => x.FinanceYearStartMonth).FirstOrDefault();
            return startMonth;
        }

        public void SaveProjectFilter(string projectId, string userId, int filter, string categoryId, string accountId, Nullable<DateTime> fromDate, Nullable<DateTime> endDate)
        {
            Entities.ProjectMember member = this.GetProjectMember(projectId, userId);
            if(member != null)
            {
                member.ViewFilterByType = filter;
                member.ViewFilterByCategory = categoryId;
                member.ViewFilterByAccount = accountId;
                member.ViewFilterFromDate = fromDate;
                member.ViewFilterEndDate = endDate;
            }
        }

        public string GetProjectTitle(string projectId)
        {
            string projectTitle = this.List.Where(x => x.Id == projectId).Select(x => x.ProjectTitle).FirstOrDefault();
            return projectTitle;
        }

        public List<AsiaMoneyer.Entities.Project> GetUserProjects(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new System.ArgumentNullException("User ID cannot be null");

            List<Entities.Project> projects = this.DbContext.ProjectMemberDbSet.Where(x => x.Client.UserId == userId && x.PermissionId == (int)Commons.Constants.PROJECT_PERMISSION.OWNER && x.IsArchived == false && x.Project.IsDeleted == false)
                                                .Select(x => x.Project)
                                                .ToList();

            return projects;

        }

        public List<AsiaMoneyer.Entities.Project> GetUserSharedProjects(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new System.ArgumentNullException("User ID cannot be null");

            List<Entities.Project> projects = this.DbContext.ProjectMemberDbSet.Where(x => x.Client.UserId == userId && x.PermissionId == (int)Commons.Constants.PROJECT_PERMISSION.MEMBER && x.IsArchived == false && x.Project.IsDeleted == false)
                                    .Select(x => x.Project)
                                    .ToList();

            return projects;
        }

        public List<AsiaMoneyer.Entities.Project> GetAllArchivedProjects(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new System.ArgumentNullException("User ID cannot be null");

            List<Entities.Project> projects = this.DbContext.ProjectMemberDbSet.Where(x => x.Client.UserId == userId && x.IsArchived == true && x.Project.IsDeleted == false)
                                    .Select(x => x.Project)
                                    .ToList();

            return projects;
        }

        public List<AsiaMoneyer.Entities.Project> GetFavoriteProjects(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new System.ArgumentNullException("User ID cannot be null");

            List<Entities.Project> projects = this.DbContext.ProjectMemberDbSet.Where(x => x.Client.UserId == userId && x.IsFavorite == true && x.Project.IsDeleted == false)
                                    .Select(x => x.Project)
                                    .ToList();

            return projects;
        }

        #endregion
    }
}
