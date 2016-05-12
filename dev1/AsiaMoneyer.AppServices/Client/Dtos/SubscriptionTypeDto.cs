using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client.Dtos
{
    public class SubscriptionTypeDto
    {
        public int Id { get; set; }
        public string SubscriptionTypeName { get; set; }
        public string SubscriptionTypeTitle { get; set; }
        public string SubscriptionTypeDesc { get; set; }
        public byte MonthValue { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
