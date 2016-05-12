using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Billing
{
    public interface IProductPriceRepository : IRepository<AsiaMoneyer.Entities.ProductPrice, string>
    {
        List<Entities.ProductPrice> GetProductPricesFromProductId(string productId);
    }
}
