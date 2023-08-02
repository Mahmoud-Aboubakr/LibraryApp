using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(100)]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(200)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}
