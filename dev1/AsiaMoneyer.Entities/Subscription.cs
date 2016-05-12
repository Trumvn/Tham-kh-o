using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("Subscriptions")]
    public partial class Subscription : Entity<string>
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

        [ForeignKey("SubscriptionTypeId")]
        public virtual SubscriptionType SubscriptionType { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
