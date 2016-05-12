using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaMoneyer.EventBus
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;

        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public void Publish<T>(T eventMessage)
        {
            var subscriptions = _subscriptionService.GetSubscriptions<T>();
            subscriptions.ToList().ForEach(x => PublishToConsumer(x, eventMessage));
        }

        private static void PublishToConsumer<T>(IConsumer<T> x, T eventMessage)
        {
            try
            {
                x.Handle(eventMessage);
            }
            catch(Exception)
            {
            }
            finally
            {
                var instance = x as IDisposable;
                if(instance != null)
                {
                    instance.Dispose();
                }
            }
        }
    }
}