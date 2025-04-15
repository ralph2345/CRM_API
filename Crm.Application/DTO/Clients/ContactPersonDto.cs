using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class ContactPersonDto
    {
        public string? ContactName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public string? DirectEmail { get; set; }
        public string? DirectPhone { get; set; }
    }
}
