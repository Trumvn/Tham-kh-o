using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using AsiaMoneyer.EntityFramework;

namespace AsiaMoneyer
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(AsiaMoneyerCoreModule))]
    public class AsiaMoneyerDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<AsiaMoneyerDbContext>(null);
        }
    }
}
