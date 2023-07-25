using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public string CustomerId { get; set; }

        public string TotalPrice { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
