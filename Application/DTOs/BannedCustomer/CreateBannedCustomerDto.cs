using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.BannedCustomer
{
    public class CreateBannedCustomerDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime? BanDate { get; set; } = null;
        public int EmpId { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
