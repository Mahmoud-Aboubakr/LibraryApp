using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateBookOrderDetailsDto
    {
        public string OrderId { get; set; }
        public string BookId { get; set; }
        public string Quantity { get; set; }
    }
}
