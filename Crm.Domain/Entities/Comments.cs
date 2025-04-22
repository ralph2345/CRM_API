using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crm.Domain.Entities
{
    public class Comments
    {
        [Key]
        public int CommentId { get; set; }
        public string? Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        [ForeignKey("ClientDetails")]
        public int ClientDetailsId { get; set; }
        public ClientDetails ClientDetails { get; set; } = null!;
    }
}
