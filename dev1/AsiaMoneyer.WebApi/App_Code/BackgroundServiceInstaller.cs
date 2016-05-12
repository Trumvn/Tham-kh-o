using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using AsiaMoneyer;
using AsiaMoneyer.EntityFramework;

public class BackgroundServiceInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        container.Register(
            Component.For<AsiaMoneyer.SystemSettings.ISysSettingAppService>().ImplementedBy<AsiaMoneyer.SystemSettings.SysSettingAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Project.IRecurringTransactionAppService>().ImplementedBy<AsiaMoneyer.Project.RecurringTransactionAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Project.IProjectAppService>().ImplementedBy<AsiaMoneyer.Project.ProjectAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Project.ITransactionAppService>().ImplementedBy<AsiaMoneyer.Project.TransactionAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Project.IAccountAppService>().ImplementedBy<AsiaMoneyer.Project.AccountAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Project.ICategoryAppService>().ImplementedBy<AsiaMoneyer.Project.CategoryAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Client.IClientAppService>().ImplementedBy<AsiaMoneyer.Client.ClientAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Client.ISubscriptionAppService>().ImplementedBy<AsiaMoneyer.Client.SubscriptionAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Client.IBillingAppService>().ImplementedBy<AsiaMoneyer.Client.BillingAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Faq.IFaqAppService>().ImplementedBy<AsiaMoneyer.Faq.FaqAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Application.IApplicationAppService>().ImplementedBy<AsiaMoneyer.Application.ApplicationAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.AuditLog.IAuditLogAppService>().ImplementedBy<AsiaMoneyer.AuditLog.AuditLogAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.UserComments.IUserCommentAppService>().ImplementedBy<AsiaMoneyer.UserComments.UserCommentAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Messaging.IAutoMessagingDatabindingHelperAppService>().ImplementedBy<AsiaMoneyer.Messaging.AutoMessagingDatabindingHelperAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Messaging.IAutoMessagingMessageAppService>().ImplementedBy<AsiaMoneyer.Messaging.AutoMessagingMessageAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Messaging.IMessagingProvider>().ImplementedBy<AsiaMoneyer.Messaging.GoogleEmailProvider>().LifestyleTransient(),
            Component.For<AsiaMoneyer.Messaging.IAutoMessagingAppService>().ImplementedBy<AsiaMoneyer.Messaging.MessagingQueueAppService>().LifestyleTransient(),
            Component.For<AsiaMoneyer.EventBus.IEventPublisher>().ImplementedBy<AsiaMoneyer.EventBus.EventPublisher>().LifestyleTransient(),
            Component.For<AsiaMoneyer.EventBus.ISubscriptionService>().ImplementedBy<AsiaMoneyer.EventBus.EventSubscriptions>().LifestyleTransient()
            );
    }
}
