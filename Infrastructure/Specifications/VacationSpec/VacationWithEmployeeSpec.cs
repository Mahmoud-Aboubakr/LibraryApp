using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.VacationSpec
{
    public class VacationWithEmployeeSpec : EntitySpec<Vacation>
    {
        public VacationWithEmployeeSpec()
        {
            AddInclude(v => v.Employee);
        }

        public VacationWithEmployeeSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(v => v.Employee);
        }
    }
}
