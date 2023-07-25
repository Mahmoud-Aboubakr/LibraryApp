using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Publisher
{
    public class ReadPublisherDto
    {
        public int Id { get; set; }
        public string PublisherName { get; set; }
        public string PublisherPhoneNumber { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
    }
}
