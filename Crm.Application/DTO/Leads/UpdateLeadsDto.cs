using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Leads
{
    public class UpdateLeadsDto
    {
        public string? LeadSource { get; set; }
        public string? Status { get; set; }
        public string? DealName { get; set; }
        public Decimal? DealValue { get; set; }
        public string? Currency { get; set; }
        public string? Stage { get; set; }
        public string? AssignedSalesRep { get; set; }
        public string? DealStatus { get; set; }
        public Decimal? EstimatedValue { get; set; }
        public Decimal? Discount { get; set; }
        public string? PaymentTerms { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
