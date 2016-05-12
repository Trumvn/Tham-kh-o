using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("SubscriptionTypes")]
    public class SubscriptionType : Entity<int>
    {
        public SubscriptionType()
        {
            this.Subscriptions = new HashSet<Subscription>();
        }

        public string SubscriptionTypeName { get; set; }
        public string SubscriptionTypeTitle { get; set; }
        public string SubscriptionTypeDesc { get; set; }
        public byte MonthValue { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }

    }
}
