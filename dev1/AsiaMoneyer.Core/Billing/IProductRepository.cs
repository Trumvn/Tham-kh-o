using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Billing
{
    public interface IProductRepository : IRepository<AsiaMoneyer.Entities.Product, string>
    {
        List<Entities.Product> GetUserUpgradeProducts(string userId);
        List<Entities.Product> GetAvailableUpgradeProductsFromLevel(int level);
        Entities.Product GetAvailavleStartLevelProduct();
        int GetSubscriptionTypeByProductPriceId(string productPriceId);
        int GetMaxUpgradeLevel();        
    }
}
