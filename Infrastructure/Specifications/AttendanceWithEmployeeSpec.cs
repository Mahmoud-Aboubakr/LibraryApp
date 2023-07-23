using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class AttendanceWithEmployeeSpec : EntitySpec<Attendence>
    {
        public AttendanceWithEmployeeSpec()
        {
            AddInclude(A => A.Employee);
        }
        public AttendanceWithEmployeeSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(A => A.Employee);
        }
    }
}
