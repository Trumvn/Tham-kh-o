using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsiaMoneyer.EventBus
{
    public interface IConsumer<T>
    {
        void Handle(T eventMessage);
    }
}