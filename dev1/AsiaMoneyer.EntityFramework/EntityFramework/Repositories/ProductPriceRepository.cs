using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Billing;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class ProductPriceRepository : AsiaMoneyerRepositoryBase<Entities.ProductPrice>, IProductPriceRepository
    {
        public ProductPriceRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public List<Entities.ProductPrice> GetProductPricesFromProductId(string productId)
        {
            return List.Where(x => x.ProductId == productId && x.IsActive == true && x.IsDeleted == false).ToList();
        }
    }
}
