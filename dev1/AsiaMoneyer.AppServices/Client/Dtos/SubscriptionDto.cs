using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client.Dtos
{
    public class SubscriptionDto
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public Nullable<DateTime> NextInvoiceDate { get; set; }
        public Nullable<DateTime> LastInvoiceDate { get; set; }
        public Nullable<DateTime> LastPaymentDate { get; set; }
        public Nullable<DateTime> ValidDate { get; set; }
        public Nullable<DateTime> ExpiredDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public string InvoiceId { get; set; }
        public string NextInvoiceId { get; set; }
        public bool CanUpgrade { get; set; }

        public ProductDto Product { get; set; }
        public SubscriptionTypeDto SubscriptionType { get; set; }
    }
}
