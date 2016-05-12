using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("Invoices")]
    public partial class Invoice : Entity<string>
    {
        public string UserId { get; set; }
        public string SubscriptionId { get; set; }
        public string ProductPriceId { get; set; }
        public Nullable<int> PaymentMethodId { get; set; }
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

        [ForeignKey("SubscriptionId")]
        public virtual Subscription Subscription { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
