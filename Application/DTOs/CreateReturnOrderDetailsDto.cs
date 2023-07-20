using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateReturnOrderDetailsDto
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int ReturnedOrderId { get; set; }
        public int BookId { get; set; }
    }
}
