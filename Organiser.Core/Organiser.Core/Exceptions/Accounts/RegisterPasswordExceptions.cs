namespace Organiser.Core.Exceptions.Accounts
{
    public class RegisterExceptions : Exception
    {
        public RegisterExceptions(string message) : base(message)
        {
        }
    }

    public class RegisterUserNameIsEmptyException : RegisterExceptions
    {
        public RegisterUserNameIsEmptyException(string message) : base(message)
        {
        }
    }

    public class RegisterUserNameIsOver100Exception : RegisterExceptions
    {
        public RegisterUserNameIsOver100Exception(string message) : base(message)
        {
        }
    }

    public class RegisterEmailIsEmptyException : RegisterExceptions
    {
        public RegisterEmailIsEmptyException(string message) : base(message)
        {
        }
    }

    public class RegisterEmailIsOver100Exception : RegisterExceptions
    {
        public RegisterEmailIsOver100Exception(string message) : base(message)
        {
        }
    }

    public class RegisterPasswordIsEmptyException : RegisterExceptions
    {
        public RegisterPasswordIsEmptyException(string message) : base(message)
        {
        }
    }

    public class RegisterPasswordNoNumbersException : RegisterExceptions
    {
        public RegisterPasswordNoNumbersException(string message) : base(message)
        {
        }
    }

    public class RegisterPasswordNoUpperCaseException : RegisterExceptions
    {
        public RegisterPasswordNoUpperCaseException(string message) : base(message)
        {
        }
    }

    public class RegisterPasswordNoLowerCaseException : RegisterExceptions
    {
        public RegisterPasswordNoLowerCaseException(string message) : base(message)
        {
        }
    }

    public class RegisterPasswordNoSpecialSignsException : RegisterExceptions
    {
        public RegisterPasswordNoSpecialSignsException(string message) : base(message)
        {
        }
    }

    public class RegisterPasswordNo8charactersException : RegisterExceptions
    {
        public RegisterPasswordNo8charactersException(string message) : base(message)
        {
        }
    }

    public class RegisterUserNameIsFoundException : RegisterExceptions
    {
        public RegisterUserNameIsFoundException(string message) : base(message)
        {
        }
    }
}