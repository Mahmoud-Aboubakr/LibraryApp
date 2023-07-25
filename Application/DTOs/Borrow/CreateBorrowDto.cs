using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Borrow
{
    public class CreateBorrowDto
    {
        public string CustomerId { get; set; }
        public string BookId { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

    }
}
