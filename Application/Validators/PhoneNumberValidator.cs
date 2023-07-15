using Application.Interfaces;


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
