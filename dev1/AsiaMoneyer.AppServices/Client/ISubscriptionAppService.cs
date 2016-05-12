using AsiaMoneyer.Client.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client
{
    public interface ISubscriptionAppService : IAppService
    {
        bool CreateSubscription(Dtos.SubscriptionDto subscriptionDto);
        SubscriptionDto GetUserCurrentSubscription(string userId);
        List<ProductDto> GetUserUpgradeProducts(string userId);
        bool CreateStartSubscriptionForNewUser(string userId);
        InvoiceDto RegisterNewSubscription(string productId, string priceId, string userId);
        InvoiceDto ProceedPurchaseWithPaymentMethod(string invoiceId, int paymentMethodId);
        void PaymentSuccess(PaymentObjectDto paymentObjectDto);
    }
}
