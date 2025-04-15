using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Crm.Domain.Entities
{
    public class ClientDetails
    {
        [Key]
        public int ClientDetailsId { get; set; }
        public string? LeadSources { get; set; }
        public string? ClientType { get; set; }
        public ICollection<Comments> Notes { get; set; } = new List<Comments>();


        [ForeignKey("Clients")]
        public int ClientID { get; set; }
        public Clients Clients { get; set; } = null!;
        

    }
}
