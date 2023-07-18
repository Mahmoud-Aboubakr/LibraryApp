using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class EmployeeTypeValidator : IEmployeeTypeValidator
    {
        public bool IsValidEmployeeType(byte EmpType)
        {
            int[] validTypes = { 0, 1, 2, 3 };
            return validTypes.Contains(EmpType);
        }
    }
}
