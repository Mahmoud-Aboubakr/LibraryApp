

namespace Application.Interfaces
{
    public interface INumbersValidator
    {
        public bool IsValidDecimal(string num);
        public bool IsValidInt(string num);
    }
}
