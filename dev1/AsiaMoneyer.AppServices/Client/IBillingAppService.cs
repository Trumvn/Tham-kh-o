using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client
{
    public interface IBillingAppService : IAppService
    {
        List<Dtos.PaymentMethodDto> GetPaymentMethods();
        Dtos.SubscriptionDto GetInvoiceProductPricesInSubscription(Dtos.PaymentObjectDto paymentObjectDto);
        Dtos.InvoiceDto UpdateInvoiceProductPrice(Dtos.PaymentObjectDto paymentObjectDto);
    }
}
