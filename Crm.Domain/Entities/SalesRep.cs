using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain.Entities
{
    public class SalesRep
    {
        [Key]
        public int SalesRepId { get; set; }
        public string? AssignedSalesRep { get; set; }
        public DateOnly? FollowUpDate { get; set; }
        public DateOnly? NextAction { get; set; }
        public DateOnly? LastContactDate { get; set; }

        [ForeignKey("LeadId")]
        public LeadTbl LeadTbl { get; set; } = null!;
        public int LeadId { get; set; } 
    }
}
