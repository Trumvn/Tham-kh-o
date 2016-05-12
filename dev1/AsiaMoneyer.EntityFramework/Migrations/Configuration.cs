using System.Data.Entity.Migrations;

namespace AsiaMoneyer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AsiaMoneyer.EntityFramework.AsiaMoneyerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AsiaMoneyer";
        }

        protected override void Seed(AsiaMoneyer.EntityFramework.AsiaMoneyerDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
