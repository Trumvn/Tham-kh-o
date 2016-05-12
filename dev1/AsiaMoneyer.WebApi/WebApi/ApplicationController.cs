using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AsiaMoneyer.WebApi.Controllers
{
    [Authorize]
    public class ApplicationController : BaseApiController
    {
        Application.IApplicationAppService _applicationAppService;

        public ApplicationController(Application.IApplicationAppService applicationAppService)
        {
            this._applicationAppService = applicationAppService;
        }

        #region Project
        public IHttpActionResult GetNavigator()
        {
            var user = AppUserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                var l = this._applicationAppService.GetNavigator(user.Id);
                return Ok(l);
            }

            return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Empty);
        }
        #endregion
    }
}