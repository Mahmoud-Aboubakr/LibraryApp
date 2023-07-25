using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.ReturnedOrder
{
    public class CreateReturnedOrderDto
    {
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }

        public int OriginOrderId { get; set; }
        public int CustomerId { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
