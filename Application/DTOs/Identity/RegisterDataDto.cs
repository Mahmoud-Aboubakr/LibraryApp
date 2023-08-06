using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Identity
{
    public class RegisterDataDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(100)]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50)]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> Roles { get; set; }
    }
}
