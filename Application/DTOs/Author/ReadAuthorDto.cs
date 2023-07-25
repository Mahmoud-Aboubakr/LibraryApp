using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Author
{
    public class ReadAuthorDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPhoneNumber { get; set; }
        public decimal? AuthorProfits { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
    }
}
