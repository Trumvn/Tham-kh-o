using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client.Dtos
{
    public class PaymentMethodDto
    {
        public int Id { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodTitle { get; set; }
        public string PaymentMethodDesc { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }        
    }
}
