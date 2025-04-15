using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Crm.Domain.Entities
{
    public class CompanyDetails
    {
        [Key]
        public int CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public string? IndustryType { get; set; }
        public string? BusinessRegNumber { get; set; }
        public string? CompanySize { get; set; }
        public string? City { get; set; }
        public string? StateProvince { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }


        [ForeignKey("ClientID")]
        public int ClientID { get; set; }
        public Clients Clients { get; set; } = null!;
    }
}
