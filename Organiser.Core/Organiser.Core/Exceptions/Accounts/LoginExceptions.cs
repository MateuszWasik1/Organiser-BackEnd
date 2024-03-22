namespace Organiser.Core.Exceptions.Accounts
{
    public class LoginExceptions : Exception
    {
        public LoginExceptions(string message) : base(message)
        {
        }
    }

    public class LoginOrUserNotFoundExceptions : LoginExceptions
    {
        public LoginOrUserNotFoundExceptions(string message) : base(message)
        {
        }
    }
}