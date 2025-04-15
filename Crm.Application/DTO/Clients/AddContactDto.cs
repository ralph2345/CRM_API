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

        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid Phone Number.")]
        public string? DirectPhoneNumber { get; set; }
    }
}
