using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.BookOrderDetails
{
    public class CreateBookOrderDetailsDto
    {
        public string OrderId { get; set; }
        public string BookId { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
