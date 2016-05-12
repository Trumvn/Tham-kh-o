using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Billing
{
    public interface IInvoiceRepository : IRepository<AsiaMoneyer.Entities.Invoice, string>
    {
        Entities.Invoice GetSubscriptionInvoiceByInvoiceDate(string subscriptionId, DateTime invoiceDate);
    }
}
