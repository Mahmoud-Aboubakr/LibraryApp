using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ReturnedOrder
{
    public class ReadReturnedOrderWithDetailsDto
    {
        public int Id { get; set; }
        public int OriginOrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }

        public string CustomerName { get; set; }

        public DateTime? CreatedDate { get; set; } 
    }
}
