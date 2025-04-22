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
        //delete full name to industry if frontend stick to get client data via client id 
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? Industry { get; set; }
        public string? LeadSource { get; set; }
        public string? Status { get; set; }

        public int? ClientID { get; set; }

        public AddDealsDto? Deals { get; set; } 
        public AddPaymentDto? Payment { get; set; } 
    }
}
