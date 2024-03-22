namespace Organiser.Core.Exceptions.Accounts
{
    public class BugsExceptions : Exception
    {
        public BugsExceptions(string message) : base(message)
        {
        }
    }

    public class BugNotFoundExceptions : BugsExceptions
    {
        public BugNotFoundExceptions(string message) : base(message)
        {
        }
    }

    public class BugTitleRequiredExceptions : BugsExceptions
    {
        public BugTitleRequiredExceptions(string message) : base(message)
        {
        }
    }

    public class BugTitleMax200Exceptions : BugsExceptions
    {
        public BugTitleMax200Exceptions(string message) : base(message)
        {
        }
    }

    public class BugTextRequiredExceptions : BugsExceptions
    {
        public BugTextRequiredExceptions(string message) : base(message)
        {
        }
    }

    public class BugTextMax4000Exceptions : BugsExceptions
    {
        public BugTextMax4000Exceptions(string message) : base(message)
        {
        }
    }
}