using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Crm.Application.DTO.Clients
{
    //this is the list on the table of clients
    public class ClientsDto
    {
        public int ClientId { get; set; }
        public string? PhotoLink{ get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? WebsiteURL { get; set; }
        public CompanyDetailsDto? CompanyDetails { get; set; }
        public List<ContactPersonDto>? ContactPerson { get; set; }//Allow multiple contact persons
        public ClientDetailsDto? ClientDetails { get; set; }
    }
}
