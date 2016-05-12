using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("PaymentMethods")]
    public partial class PaymentMethod : Entity<int>
    {
        public string PaymentMethodName { get; set; }
        public string PaymentMethodTitle { get; set; }
        public string PaymentMethodDesc { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }        
    }
}
