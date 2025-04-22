using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Leads
{
    public class LeadsDto
    {
        public int LeadId { get; set; }
        public string? FullName{ get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? Industry { get; set; }
        public string? LeadSource { get; set; }
        public string? Status { get; set; }
        public DateOnly? DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DealsDto? Deals { get; set; } 
        public PaymentDto? Payment { get; set; } 
      
    }
}
