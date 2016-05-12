using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaMoneyer.EventBus
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage);
    }
}