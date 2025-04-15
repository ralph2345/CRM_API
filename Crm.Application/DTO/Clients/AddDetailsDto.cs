using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class AddDetailsDto
    {
        public string? LeadSources { get; set; }
        public string? ClientType { get; set; }
        public List<string>? Notes { get; set; }
    }
}
