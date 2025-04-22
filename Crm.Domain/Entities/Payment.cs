using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public Decimal? EstimatedValue { get; set; }
        public Decimal? Discount { get; set; }
        public Decimal? TotalPrice { get; set; }
        public string? PaymentTerms { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PaymentStatus { get; set; }

        [ForeignKey("LeadId")]
        public LeadTbl LeadTbl { get; set; } = null!;
        public int LeadId { get; set; }

    }
}
