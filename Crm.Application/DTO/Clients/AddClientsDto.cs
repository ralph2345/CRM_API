using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class AddClientsDto
    {
        public string? PhotoLink { get; set; }
        public required string FirstName { get; set; }
        public required string MiddleName { get; set; }
        public required string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; }

        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Phone number must start with '09' and be exactly 11 digits.")]
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        public AddCompanyDto? Company { get; set; }

        //download Swashbuckle.AspNetCore.Annotations to add a readable description to the Swagger UI
        /*[SwaggerSchema(
            Description = "List of Contact Persons",
            Type = "array",
            Items = new { Type = "object" }
          )]*/
        public List<AddContactDto>? Contact { get; set; }//allow multiple contacts
        public AddDetailsDto? Details { get; set; }
   
    }
}
