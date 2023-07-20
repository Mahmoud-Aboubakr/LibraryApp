using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReadReturnedOrderDto
    {
        public int Id { get; set; }
        public int OriginOrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
