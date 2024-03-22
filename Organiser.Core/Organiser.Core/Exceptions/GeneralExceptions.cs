﻿namespace Organiser.Core.Exceptions.Accounts
{
    public class GeneralExceptions : Exception
    {
        public GeneralExceptions(string message) : base(message)
        {
        }
    }

    public class UserNotFoundExceptions : GeneralExceptions
    {
        public UserNotFoundExceptions(string message) : base(message)
        {
        }
    }
}