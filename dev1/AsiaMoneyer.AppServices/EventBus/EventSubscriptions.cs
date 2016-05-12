using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaMoneyer.EventBus
{
    public class EventSubscriptions : ISubscriptionService
    {
        private static IWindsorContainer IoC;

        public static void Initizilize(IWindsorContainer ioc)
        {
            EventSubscriptions.IoC = ioc;
        }

        public static void Add<T>()
        {
            var consumerType = typeof(T);
            var list = consumerType.GetInterfaces().Where(x => x.IsGenericType).Where(x => x.GetGenericTypeDefinition() == typeof(IConsumer<>))
                .ToList();

            /*consumerType.GetInterfaces().Where(x => x.IsGenericType).Where(x => x.GetGenericTypeDefinition() == typeof(IConsumer<>))
                .ToList()
                .ForEach(x => container.Register(Component.For<x>().ImplementedBy<consumerType>().Name(consumerType.FullName).LifestyleTransient()));
            //.ForEach(x => IoC.Container.RegisterType(x, consumerType, consumerType.FullName));
             * */
            list.ForEach(x => IoC.Register(Component.For(x).ImplementedBy(typeof(T)).Named(consumerType.FullName).LifestyleTransient()));
            //IoC.Register(Component.For(typeof(IConsumer<T>)).ImplementedBy(typeof(T)).Named(consumerType.FullName).LifestyleTransient());
        }

        public IEnumerable<IConsumer<T>> GetSubscriptions<T>()
        {
            var consumers = IoC.ResolveAll(typeof(IConsumer<T>));
            return consumers.Cast<IConsumer<T>>();
        }
    }

    
}