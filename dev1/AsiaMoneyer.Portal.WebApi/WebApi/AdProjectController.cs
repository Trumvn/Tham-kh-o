using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AsiaMoneyer.Project.Dtos;
using Microsoft.AspNet.Identity;

namespace AsiaMoneyer.Portal.WebApi.Controllers
{
    [Authorize]
    public class AdProjectController : BaseApiController
    {
        Project.IProjectAppService _projectAppService;
        Client.IClientAppService _clientAppService;

        public AdProjectController(Project.IProjectAppService projectAppService, Client.IClientAppService clientAppService)
        {
            this._projectAppService = projectAppService;
            this._clientAppService = clientAppService;
        }

        [HttpPost]
        public IHttpActionResult SearchProjects(Project.Dtos.ProjectFilterDto filter)
        {
            if (filter != null)
            {
                var l = this._projectAppService.SearchProjects(filter);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);

        }
    }
}