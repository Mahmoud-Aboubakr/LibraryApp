using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Order
{
    public class ReadOrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
    }
}
