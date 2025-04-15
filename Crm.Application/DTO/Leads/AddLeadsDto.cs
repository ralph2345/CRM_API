using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Crm.Application.DTO.Leads
{
    public class AddLeadsDto
    {
        public string? LeadSource { get; set; }
        public string? LeadStatus { get; set; }
        public string? SalesStage { get; set; }
        public string? Product { get; set; }
        public string? DealName { get; set; }
        public DateOnly? ExpectedCloseDate { get; set; }

        public AddSalesRepDto? SalesRep { get; set; } 
        public AddPaymentDto? Payment { get; set; } 
    }
}
