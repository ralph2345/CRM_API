using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain.Entities
{
    public class ContactPerson
    {
        [Key]
        public int ContactID { get; set; }
        public string? ContactName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public string? DirectEmail { get; set; }
        public string? DirectPhone { get; set; }

        [ForeignKey("ClientID")]
        public int ClientID { get; set; }
        public Clients Clients { get; set; } = null!;
    }
}
