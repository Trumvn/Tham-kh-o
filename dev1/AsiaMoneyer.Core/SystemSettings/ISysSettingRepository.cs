using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.SystemSettings
{
    public interface ISysSettingRepository : IRepository<AsiaMoneyer.Entities.SysSetting, int>
    {
    }
}
