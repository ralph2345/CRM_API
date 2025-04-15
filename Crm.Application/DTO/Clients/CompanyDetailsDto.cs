using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class CompanyDetailsDto
    {
        public string? CompanyName { get; set; }
        public string? IndustryType { get; set; }
        public string? BusinessRegNumber { get; set; }
        public string? CompanySize { get; set; }
        public string? CompanyAddress { get; set; }
    }
}
