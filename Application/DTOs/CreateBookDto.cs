using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateBookDto
    {
        public string BookTitle { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
    }
}
