namespace Application.Interfaces.IValidators
{
    public interface INumbersValidator
    {
        public bool IsValidDecimal(string num);
        public bool IsValidInt(string num);
    }
}
