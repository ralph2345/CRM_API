using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class UpdateClientsDto
    {
        public string? PhotoLink { get; set; }
        public string? FullName { get; set; }

        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Phone number must start with '09' and be exactly 11 digits.")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
        public string? WebsiteURL { get; set; }
        public string? ContactName { get; set; }
        public string? JobTitle{ get; set; }
        public string? Department { get; set; }

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format.")]
        public string? DirectEmail { get; set; }

        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Phone number must start with '09' and be exactly 11 digits.")]
        public string? DirectPhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? IndustryType { get; set; }
        public string? BusinessRegNumber { get; set; }
        public string? CompanySize { get; set; }
        public string? CompanyAddress { get; set; }
    }
}
