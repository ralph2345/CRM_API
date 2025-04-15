using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class UpdateClientsDto
    {
        public string? PhotoLink { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? WebsiteURL { get; set; }
        public string? ContactName { get; set; }
        public string? JobTitle{ get; set; }
        public string? Department { get; set; }
        public string? DirectEmail { get; set; }
        public string? DirectPhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? IndustryType { get; set; }
        public string? BusinessRegNumber { get; set; }
        public string? CompanySize { get; set; }
        public string? CompanyAddress { get; set; }
    }
}
