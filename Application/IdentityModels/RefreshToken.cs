using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IdentityModels
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiresOn;
        [Column(TypeName = "datetime")] 
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")] 
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
