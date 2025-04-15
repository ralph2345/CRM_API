using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Leads
{
    public class AddSalesRepDto
    {
        public string? AssignedSalesRep { get; set; }
        public DateOnly? FollowUpDate { get; set; }
        public DateOnly? NextAction { get; set; }
        public DateOnly? LastContactDate { get; set; }
    }
}
