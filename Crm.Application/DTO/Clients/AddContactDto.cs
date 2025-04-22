using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class AddContactDto
    {
        public string? ContactName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? DirectEmail { get; set; }

        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Phone number must start with '09' and be exactly 11 digits.")]
        public string? DirectPhoneNumber { get; set; }
    }
}
