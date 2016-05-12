using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("Products")]
    public partial class Product : Entity<string>
    {
        public Product()
        {
            this.ProductPrices = new HashSet<ProductPrice>();
        }

        public string ProductName { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDesc { get; set; }
        public short UpgradeLevel { get; set; }
        public Nullable<int> MaxProject { get; set; }
        public Nullable<int> MaxUserMember { get; set; }
        public Nullable<DateTime> ValidDate { get; set; }
        public Nullable<DateTime> ExpiredDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }
}
