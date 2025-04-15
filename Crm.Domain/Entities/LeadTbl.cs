using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Crm.Domain.Entities
{
    public class LeadTbl
    {
        [Key]
        public int LeadId { get; set; }
        public string? DealName { get; set; }
        public string? LeadSource { get; set; }
        public string? LeadStatus { get; set; }
        public string? SalesStage { get; set; }
        public string? Product { get; set; }
        public DateOnly? ExpectedCloseDate { get; set; }

        public SalesRep? SalesRep { get; set; } = null!;
        public Payment? Payment { get; set; } = null!;

    }
}
