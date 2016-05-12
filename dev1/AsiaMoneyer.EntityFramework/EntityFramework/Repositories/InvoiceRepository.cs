using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Billing;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class InvoiceRepository : AsiaMoneyerRepositoryBase<Entities.Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public Entities.Invoice GetSubscriptionInvoiceByInvoiceDate(string subscriptionId, DateTime invoiceDate)
        {
            Entities.Invoice invoice = this.List.Where(x => x.SubscriptionId == subscriptionId && x.InvoiceDate == invoiceDate).FirstOrDefault();

            return invoice;
        }
    }
}
