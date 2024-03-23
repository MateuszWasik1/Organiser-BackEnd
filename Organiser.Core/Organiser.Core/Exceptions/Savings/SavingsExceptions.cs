namespace Organiser.Core.Exceptions.Savings
{
    public class SavingsExceptions : Exception
    {
        public SavingsExceptions(string message) : base(message)
        {
        }
    }

    public class SavingNotFoundException : SavingsExceptions
    {
        public SavingNotFoundException(string message) : base(message)
        {
        }
    }

    public class SavingAmountLessThan0Exception : SavingsExceptions
    {
        public SavingAmountLessThan0Exception(string message) : base(message)
        {
        }
    }
}