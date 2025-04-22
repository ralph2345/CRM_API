using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Leads
{
    public class PaymentDto
    {
        public Decimal? EstimatedValue { get; set; }
        public Decimal? Discount { get; set; }
        public Decimal? TotalPrice { get; set; }
        public string? PaymentTerms { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
