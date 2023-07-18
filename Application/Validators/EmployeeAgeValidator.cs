using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class EmployeeAgeValidator : IEmployeeAgeValidator
    {
        public bool IsValidEmployeeAge(int age)
        {
            return age >=20 && age <= 60;
        }
    }
}
