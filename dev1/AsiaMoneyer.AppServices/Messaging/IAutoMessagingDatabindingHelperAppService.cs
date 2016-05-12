using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Messaging
{
    public interface IAutoMessagingDatabindingHelperAppService : IAppService
    {
        String bind(String source, Dictionary<string, string> values);
    }
}
