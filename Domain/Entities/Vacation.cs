using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vacation : BaseEntity
    {
        public int EmpId { get; set; }
        public DateFormat DayDate { get; set; }
        public bool NormalVacation { get; set; }
        public bool UrgentVacation { get; set; }
        public bool Absence { get; set; }
    }
}
