using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
    }
}
