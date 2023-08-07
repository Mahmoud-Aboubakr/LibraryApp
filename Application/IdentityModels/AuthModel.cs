using Application.Handlers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.IdentityModels
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        //public DateTime ExpiresOn { get; set; }
        
        [JsonIgnore]
        public string RefreshToken { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
