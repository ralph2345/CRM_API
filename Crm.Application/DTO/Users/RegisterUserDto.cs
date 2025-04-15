using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Application.DTO.Users
{
    public class RegisterUserDto
    {
        public string? PhotoLink { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Middle name is required.")]
        public required string MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public required string PhoneNumber { get; set; }

        public required string Status { get; set; } = "Active";

        [Required(ErrorMessage = "Username is required.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }

        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
