using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Vacation
{
    public class ReadVacationDetailsDto
    {
        public int Id { get; set; }
        public DateTime DayDate { get; set; }
        public bool? NormalVacation { get; set; }
        public bool? UrgentVacation { get; set; }
        public bool? Absence { get; set; }

        public int EmpId { get; set; }
        public string EmpName { get; set; }

        public DateTime? CreatedDate { get; set; } 
    }
}
