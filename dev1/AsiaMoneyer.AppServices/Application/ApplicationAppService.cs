using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Dtos;
using AsiaMoneyer.Project;

namespace AsiaMoneyer.Application
{
    public class ApplicationAppService : AppServiceBase, IApplicationAppService
    {
        public NavigatorDto GetNavigator(string userId)
        {
            NavigatorDto navDto = new NavigatorDto();

            List<AsiaMoneyer.Entities.Project> projectEntities = this.UnitOfWork.ProjectRepository.GetProjectTitleList(userId);
            List<NavigatorProjectDto> navProjectDtos = AutoMapper.Mapper.Map<List<Entities.Project>, List<NavigatorProjectDto>>(projectEntities);

            foreach (NavigatorProjectDto project in navProjectDtos)
            {
                Entities.ProjectMember member = this.UnitOfWork.ProjectRepository.GetProjectMember(project.Id, userId);
                project.User = AutoMapper.Mapper.Map<Entities.ProjectMember, Project.Dtos.ProjectMemberDto>(member);
            }
            navDto.Projects = navProjectDtos;

            return navDto;
        }
    }
}
