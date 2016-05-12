using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.Http.Controllers;


public class ControllersInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        container.Register(Classes.FromThisAssembly()
            .BasedOn<IController>()
            .LifestyleTransient());

        /*container.Register(Classes.FromThisAssembly()
            .BasedOn<IHttpController>()
            .LifestyleTransient());
        */
        /*container.Register(Classes
    .FromThisAssembly()
    .BasedOn<IHttpController>()
    .ConfigureFor<ApiController>(c => c.Properties(pi => false))
    .LifestyleTransient());*/

    }
}

