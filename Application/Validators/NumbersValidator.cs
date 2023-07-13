using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class NumbersValidator : INumbersValidator
    {
        public bool IsValidDecimal(string num)
        {
            decimal number;
            if (decimal.TryParse(num, out number))
                return true;
            else
                return false;
        }
        public bool IsValidInt(string num)
        {
            int number;
            if (int.TryParse(num, out number))
                return true;
            else
                return false;
        }
    }
}
