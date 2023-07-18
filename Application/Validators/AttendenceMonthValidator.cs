using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AttendenceMonthValidator : IAttendenceMonthValidator
    {
        public bool IsValidMonth(byte month)
        {
           return month >= 1 && month <= 12;
        }
    }
}
