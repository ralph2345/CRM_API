using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Users
{
    public class UpdateUserDto
    {
        public string? PhotoLink { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Phone number must start with '09' and be exactly 11 digits.")]
        public string? PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string? UserName { get; set; }

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
    }
}
