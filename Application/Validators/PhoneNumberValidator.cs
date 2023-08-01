using Application.Exceptions;
using Application.Interfaces.IValidators;

namespace Application.Validators
{
    public class PhoneNumberValidator : IPhoneNumberValidator
    {
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            try
            {
                int number;
                if (phoneNumber.Length != 11 || !phoneNumber.StartsWith("0") ||
                    !int.TryParse(phoneNumber, out number))
                    return false;
                else
                    return true;
            }
            catch(Exception ex)
            {
                throw new BadRequestException(ex.ToString());
            }
        }
    }
}
