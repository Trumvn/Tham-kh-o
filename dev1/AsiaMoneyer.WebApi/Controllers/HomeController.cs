using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;

namespace AsiaMoneyer.WebApi.Controllers
{
    public class HomeController : Controller
    {

        // this is Castle.Core.Logging.ILogger, not log4net.Core.ILogger
        public ILogger Logger { get; set; }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            Logger.WarnFormat("User {0} attempted login but password validation failed", "admin");

            return View();
        }
    }
}
