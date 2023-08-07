using Application.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.IdentityModels
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiresOn;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedOn { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
