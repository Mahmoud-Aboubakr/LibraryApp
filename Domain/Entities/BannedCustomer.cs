using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BannedCustomer : BaseEntity
    {
        public int Customer_Id { get; set; }
        public DateFormat BanDate { get; set; }
        public int Emp_Id { get; set; }
    }
}
