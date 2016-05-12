using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.SystemSettings
{
    public class SysSettingAppService : AppServiceBase, ISysSettingAppService
    {
        public String GetSysSetting(String key, String lang)
        {
            Entities.SysSetting setting = this.UnitOfWork.SysSettingRepositoty.List.Where(x => x.Key == key && x.Lang == lang).FirstOrDefault();
            return setting!= null? setting.Value : null;
        }
    }
}
