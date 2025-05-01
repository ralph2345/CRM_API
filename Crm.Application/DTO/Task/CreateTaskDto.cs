using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Task
{
    public class CreateTaskDto
    {
        public string? TaskTitle { get; set; }
        public string? TaskType { get; set; }

        // Use ClientId to associate with a client
        public int ClientId { get; set; }

        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
    }

}
