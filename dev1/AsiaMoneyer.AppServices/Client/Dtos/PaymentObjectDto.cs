using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client.Dtos
{
    public class PaymentObjectDto
    {
        public string PriceId { get; set; }
        public string InvoiceId { get; set; }
        public int PaymentMethodId { get; set; }
        public Nullable<DateTime> PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
    }
}
