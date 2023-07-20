using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReadReturnOrderDetailsWithDetailsDto
    {
        public int Id { get; set; }
        public int ReturnedOrderId { get; set; }
        public string CustomerName { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
