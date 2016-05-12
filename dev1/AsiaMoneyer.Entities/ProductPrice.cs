using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("ProductPrices")]
    public class ProductPrice : Entity<string>
    {
        public int SubscriptionTypeId { get; set; }
        public int TargetMarketId { get; set; }
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal TaxValue { get; set; }
        public string TaxCode { get; set; }
        public Nullable<DateTime> ValidDate { get; set; }
        public Nullable<DateTime> ExpiredDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        [ForeignKey("SubscriptionTypeId")]
        public virtual SubscriptionType SubscriptionType { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        //[ForeignKey("TargetMarketId")]
        //public virtual TargetMarket TargetMarket { get; set; }
    }
}
