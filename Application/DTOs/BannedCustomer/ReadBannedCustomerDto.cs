using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.BannedCustomer
{
    public class ReadBannedCustomerDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? BanDate { get; set; } = null;
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedDate { get; set; }

    }
}
