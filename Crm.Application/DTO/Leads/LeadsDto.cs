using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Leads
{
    public class LeadsDto
    {
        public string? LeadSource { get; set; }
        public string? LeadStatus { get; set; }
        public string? SalesStage { get; set; }
        public string? Product { get; set; }
        public string? DealName { get; set; }
        public DateOnly? ExpectedCloseDate { get; set; }
        public SalesRepDto? SalesRep { get; set; } 
        public PaymentDto? Payment { get; set; } 
    }
}
