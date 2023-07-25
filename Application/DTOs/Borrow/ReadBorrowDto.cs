using Application.Handlers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Borrow
{
    public class ReadBorrowDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime BorrowDate { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime ReturnDate { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreatedDate { get; set; }
    }
}
