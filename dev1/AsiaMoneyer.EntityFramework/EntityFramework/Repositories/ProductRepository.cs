using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Billing;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class ProductRepository : AsiaMoneyerRepositoryBase<Entities.Product>, IProductRepository
    {
        public ProductRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public Entities.Product GetAvailavleStartLevelProduct()
        {
            int startLevel = 0;
            DateTime today = DateTime.Today.AddDays(1);
            Entities.Product product = this.List.Where(x => x.UpgradeLevel == startLevel && x.IsActive == true && x.IsDeleted == false && x.ValidDate < today && (x.ExpiredDate == null || x.ExpiredDate > today)).FirstOrDefault();

            return product;
        }

        public List<Entities.Product> GetUserUpgradeProducts(string userId)
        {
            DateTime today = DateTime.Today.AddDays(1);
            List<Entities.Product> products = this.List.Where(x => x.IsActive == true && x.IsDeleted == false && x.ValidDate < today && (x.ExpiredDate == null || x.ExpiredDate > today)).ToList();
            return products;
        }

        public List<Entities.Product> GetAvailableUpgradeProductsFromLevel(int level)
        {
            DateTime today = DateTime.Today.AddDays(1);
            List<Entities.Product> products = this.List.Where(x => x.UpgradeLevel > level && x.IsActive == true && x.IsDeleted == false && x.ValidDate < today && (x.ExpiredDate == null || x.ExpiredDate > today)).OrderBy(x=>x.UpgradeLevel).ToList();
            return products;
        }

        public int GetSubscriptionTypeByProductPriceId(string productPriceId)
        {
            int subscriptionTypeId = this.DbContext.ProductPriceDbSet.Where(x => x.Id == productPriceId).Select(x=>x.SubscriptionTypeId).FirstOrDefault();
            return subscriptionTypeId;
        }

        public int GetMaxUpgradeLevel()
        {
            short maxUpgradeLevel = this.List.Where(x => x.IsActive == true && x.IsDeleted == false).Max(x=>x.UpgradeLevel);
            return maxUpgradeLevel;
        }
    }
}
