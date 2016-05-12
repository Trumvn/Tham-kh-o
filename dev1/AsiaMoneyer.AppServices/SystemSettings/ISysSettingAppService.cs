using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.SystemSettings
{
    public interface ISysSettingAppService : IAppService
    {
        String GetSysSetting(String key, String lang);
    }
}
