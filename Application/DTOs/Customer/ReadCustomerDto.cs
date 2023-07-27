using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Customer
{
    public class ReadCustomerDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
    }
}
