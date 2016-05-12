using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("PaymentGateways")]
    public partial class PaymentGateway : Entity<string>
    {
        public int PaymentTypeId { get; set; }
        public string PaymentGatewayName { get; set; }
        public string PaymentGatewayTitle { get; set; }
        public string PaymentGatewayDesc { get; set; }
        public Nullable<DateTime> ValidDate { get; set; }
        public Nullable<DateTime> ExpiredDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
