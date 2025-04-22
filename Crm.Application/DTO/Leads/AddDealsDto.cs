using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Leads
{
    public class AddDealsDto
    {
        public string? DealName { get; set; }
        public Decimal? DealValue { get; set; }
        public string? Currency { get; set; }
        public string? Stage { get; set; }
        public string? AssignedSalesRep { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}
