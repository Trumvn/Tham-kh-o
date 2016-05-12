using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using AsiaMoneyer.Security.Infrastructure;

namespace AsiaMoneyer.WebApi.Controllers
{
    [Authorize]
    public class OnlineBillingController : Controller
    {
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;
        private ApplicationSignInManager _AppSignInManager = null;
        private ApplicationGroupManager _AppGroupManager = null;

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        protected ApplicationSignInManager AppSignInManager
        {
            get
            {
                return _AppSignInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        //
        // GET: /Billing/
        public ActionResult Index()
        {
            return View();
        }
	}
}