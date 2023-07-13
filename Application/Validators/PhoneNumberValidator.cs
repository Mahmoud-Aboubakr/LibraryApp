using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class PhoneNumberValidator : IPhoneNumberValidator
    {
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            int number;
            if (phoneNumber.Length != 11 || !phoneNumber.StartsWith("0") ||
                int.TryParse(phoneNumber, out number))
                return false;
            else
                return true;
        }
    }
}
