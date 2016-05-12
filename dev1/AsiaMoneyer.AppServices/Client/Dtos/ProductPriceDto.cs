using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client.Dtos
{
    public class ProductPriceDto
    {
        public string Id { get; set; }
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

        public SubscriptionTypeDto SubscriptionType { get; set; }

        //public ProductDto Product { get; set; }

        public TargetMarket TargetMarket { get; set; }
    }
}
