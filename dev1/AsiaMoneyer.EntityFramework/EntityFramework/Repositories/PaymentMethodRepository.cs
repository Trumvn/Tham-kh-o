using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Billing;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class PaymentMethodRepository : AsiaMoneyerRepositoryBase<Entities.PaymentMethod, int>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public List<Entities.PaymentMethod> GetAvailablePaymentMethods()
        {
            return this.List.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
        }
    }
}
