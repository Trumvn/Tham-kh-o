using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Billing
{
    public interface IPaymentMethodRepository : IRepository<AsiaMoneyer.Entities.PaymentMethod, int>
    {
        List<Entities.PaymentMethod> GetAvailablePaymentMethods();
    }
}
