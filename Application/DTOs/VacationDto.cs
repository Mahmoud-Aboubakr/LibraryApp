using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class VacationDto
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? DayDate { get; set; } = null;
        public bool? NormalVacation { get; set; }
        public bool? UrgentVacation { get; set; }
        public bool? Absence { get; set; }
    }
}
