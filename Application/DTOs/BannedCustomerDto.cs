using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BannedCustomerDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime? BanDate { get; set; } = null;
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }
}
