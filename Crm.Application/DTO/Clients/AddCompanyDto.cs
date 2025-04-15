using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class AddCompanyDto
    {
        public required string CompanyName { get; set; }
        public string? IndustryType { get; set; }
        public required string BusinessRegNumber { get; set; }
        public string? CompanySize { get; set; }
        public string? City { get; set; }
        public string? StateProvince { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }
}

