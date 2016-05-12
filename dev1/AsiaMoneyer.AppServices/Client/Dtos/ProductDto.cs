using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDesc { get; set; }
        public short UpgradeLevel { get; set; }
        public int MaxProject { get; set; }
        public int MaxUserMember { get; set; }
        public Nullable<DateTime> ValidDate { get; set; }
        public Nullable<DateTime> ExpiredDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public ICollection<ProductPriceDto> ProductPrices { get; set; }
    }
}
