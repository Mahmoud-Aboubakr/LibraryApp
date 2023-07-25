using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.BannedCustomer
{
    public class CreateBannedCustomerDto
    {
        public int CustomerId { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? BanDate { get; set; } = null;
        public int EmpId { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
