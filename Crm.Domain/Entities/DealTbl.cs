using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain.Entities
{
    public class DealTbl
    {
        [Key]
        public int DealId { get; set; }
        public string? DealName { get; set; }
        public Decimal? DealValue { get; set; }
        public string? Currency { get; set; }
        public string? Stage { get; set; }
        public string? AssignedSalesRep { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }

        [ForeignKey("LeadId")]
        public LeadTbl LeadTbl { get; set; } = null!;
        public int LeadId { get; set; } 
    }
}
