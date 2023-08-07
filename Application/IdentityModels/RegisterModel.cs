using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IdentityModels
{
    public class RegisterModel
    {
        [Required, StringLength(50)]
        public string Username { get; set; }

        [EmailAddress]
        [Required, StringLength(128)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        
        [Required, StringLength(256)]
        public string Password { get; set; }

        [Required, StringLength(256)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}
