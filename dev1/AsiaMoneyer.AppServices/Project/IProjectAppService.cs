using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.AuditLog.Dtos;

namespace AsiaMoneyer.Project
{
    public interface IProjectAppService : IAppService
    {
        Dtos.GetProjectsOutput GetProjects();
        Dtos.GetProjectsOutput GetProjects(Project.Dtos.ProjectFilterDto filter);
        Dtos.ProjectDto GetProject(string projectId);
        List<Dtos.ProjectMemberDto> GetProjectMembers(string projectId);
        void CreateProject(Dtos.ProjectDto project);
        void SaveProject(Dtos.ProjectDto project);
        void SetProjectHighlightColor(Dtos.ProjectDto projectDto);
        void SetProjectArchived(Dtos.ProjectDto projectDto);
        void SetProjectFavorite(Dtos.ProjectDto projectDto);
        Dtos.ProjectHeaderDto GetProjectHeader(string projectId);
        Dtos.GetProjectsOutput FilterProjects(Dtos.ProjectFilterDto filter);
        Dtos.ProjectSearchOutput SearchProjects(Dtos.ProjectFilterDto filter);
        void DeleteProject(string projectId);
        List<AuditLogDto> GetProjectRecentActivity(string projectId, Dtos.PagingDto paging);
        List<Dtos.ProjectMemberDto> InviteMember(string projectId, string memberEmail);
        List<Dtos.ProjectMemberDto> RemoveMember(string projectMemberId);
        void AcceptInvitition(string code);
        Dtos.ProjectSummaryDto GetProjectSummary(string projectId);
        void SaveProjectFilter(string projectId, int filter, string categoryId, string accountId, Nullable<DateTime> fromDate, Nullable<DateTime> endDate);
        Dtos.TransactionFilterDto GetProjectFilter(string projectId);
        List<Dtos.ProjectHeaderDto> GetFavoriteProjectHeaders(string userId);
    }
}
