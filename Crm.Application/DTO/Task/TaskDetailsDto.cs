using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Task
{
    public class TaskDetailsDto
    {
        public int Id { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskType { get; set; }
        public string? AssignedTo { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        
    }
}
