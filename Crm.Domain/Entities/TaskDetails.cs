using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain.Entities
{
    public class TaskDetails
    {
        [Key]
        public int Id { get; set; }

        public string? TaskTitle { get; set; }
        public string? TaskType { get; set; }
        public string? AssignedTo { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public bool? IsArchived { get; set; } = false;

        [ForeignKey("ClientID")]
        public int ClientID { get; set; }
        public Clients Clients { get; set; } = null!;
      
    }
}
