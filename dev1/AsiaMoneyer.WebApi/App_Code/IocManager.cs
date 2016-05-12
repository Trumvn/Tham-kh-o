using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.Registration;
using System.Reflection;
using Castle.MicroKernel;
using Castle.Windsor.Proxy;
using System.Web.Http;

namespace AsiaMoneyer.WebApi
{
    public static class IocManager
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer IoC { get { return _container; } }

        public static void Setup()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());

            WindsorControllerFactory controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver(_container.Kernel);

            // EventBus
            EventBus.EventSubscriptions.Initizilize(IoC);
            EventBus.EventSubscriptions.Add<EventBus.NotifyAuditLog>();
        }

        public static void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }
    }
}