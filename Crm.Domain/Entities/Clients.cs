using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Crm.Domain.Entities
{
    public class Clients
    {
        [Key]
        public int ClientID { get; set; }
        public string? PhotoLink { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WebsiteURL { get; set; }
        public bool IsArchived { get; set; } = false;

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public CompanyDetails? CompanyDetails { get; set; }
        public List<ContactPerson> ContactPerson { get; set; } = new List<ContactPerson>();
        public ClientDetails? ClientDetails { get; set; }
        public ICollection<TaskDetails> TaskDetails { get; set; } = new List<TaskDetails>();
        public LeadTbl? LeadTbl { get; set; }
    }       
}
