using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client.Dtos
{
    public class InvoiceDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string SubscriptionId { get; set; }
        public int PaymentMethodId { get; set; }
        public string ProductPriceId { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public Nullable<DateTime> PaymentDate { get; set; }
        public decimal Price { get; set; }
        public decimal Quality { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
        public string TransactionId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public SubscriptionDto Subscription { get; set; }

        public UserDto User { get; set; }
    }
}
