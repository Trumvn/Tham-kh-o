using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("TargetMarkets")]
    public class TargetMarket : Entity<int>
    {
        public string TargetmMarketName { get; set; }
        public string TargetmMarketTitle { get; set; }
        public string TargetmMarketDesc { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

    }
}
