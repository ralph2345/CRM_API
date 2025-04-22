using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain.Entities
{
    public class LeadTbl
    {
        [Key]
        public int LeadId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? Industry { get; set; }
        public string? LeadSource { get; set; }
        public string? Status { get; set; }

        public DateOnly? DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [ForeignKey("ClientID")]
        public int? ClientID { get; set; }
        public Clients Clients { get; set; } = null!;
        public DealTbl? DealTbl { get; set; } = null!;
        public Payment? Payment { get; set; } = null!;

    }
}
