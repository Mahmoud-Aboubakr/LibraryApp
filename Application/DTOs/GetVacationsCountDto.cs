using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetVacationsCountDto
    {
        public int EmpId { get; set; }
        public int NormalVacationCount { get; set; }
        public int UrgentVacationCount { get; set; }
        public int AbsenceCount { get; set; }
    }
}
