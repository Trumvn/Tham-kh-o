using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AsiaMoneyer.Portal.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IocManager.Setup();

            AsiaMoneyer.Mapping.AutoMapper.doMapping();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AsiaMoneyer.Security.Infrastructure.ApplicationDbContext, AsiaMoneyer.Security.Migrations.Configuration>()); 
            Database.SetInitializer<AsiaMoneyer.Security.Infrastructure.ApplicationDbContext>(null); 

        }
    }
}
