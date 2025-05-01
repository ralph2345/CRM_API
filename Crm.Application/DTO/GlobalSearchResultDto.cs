using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO
{
    public class GlobalSearchResultDto
    {
        public required string Type { get; set; }  
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public string? CompanyName { get; set; }

        //public string? TaskType { get; set; }
        //public string? TaskTitle { get; set; }

        //public string? DealName { get; set; }
    }
}
