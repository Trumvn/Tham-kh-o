using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.Project
{
    public interface IProjectRepository : IRepository<AsiaMoneyer.Entities.Project, string>
    {
        List<AsiaMoneyer.Entities.Project> GetProjectTitleList(string userId);
        List<AsiaMoneyer.Entities.Project> GetProjects();
        List<AsiaMoneyer.Entities.Project> GetUserProjects(string userId);
        List<AsiaMoneyer.Entities.Project> GetUserSharedProjects(string userId);
        List<AsiaMoneyer.Entities.Project> GetAllArchivedProjects(string userId);
        List<AsiaMoneyer.Entities.Project> GetFavoriteProjects(string userId);

        List<AsiaMoneyer.Entities.Project> GetProjects(string userId, bool isIncludeArchived);
        void RemoveMember(string projectId, string clientId);
        string RemoveMember(string projectMemberId);
        Entities.Client GetProjectOwner(string projectId);
        Entities.ProjectMember GetProjectMember(string projectId, string userId);

        string GetProjectCurrency(string projectId);
        string GetProjectCurrencyByCategory(string categoryId);
        string GetProjectCurrencyByAccount(string accountId);

        int GetProjectFinanceYearStartMonth(string projectId);
        void SaveProjectFilter(string projectId, string userId, int filter, string categoryId, string accountId, Nullable<DateTime> fromDate, Nullable<DateTime> endDate);

        string GetProjectTitle(string projectId);
    }
}
