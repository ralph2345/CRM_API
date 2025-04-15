using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Domain.Entities
{
    public class ApiTokenModel
    {
        public int Id { get; set; }
        public string? ApiToken { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
    }
}
