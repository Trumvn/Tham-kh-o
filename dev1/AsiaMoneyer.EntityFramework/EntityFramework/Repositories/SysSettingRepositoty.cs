using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.SystemSettings;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class SysSettingRepositoty : AsiaMoneyerRepositoryBase<Entities.SysSetting, int>, ISysSettingRepository
    {
        public SysSettingRepositoty(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
