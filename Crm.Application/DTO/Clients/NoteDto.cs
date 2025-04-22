using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Clients
{
    public class NoteDto
    {
        public string? Content { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
