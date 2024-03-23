namespace Organiser.Core.Exceptions.Accounts
{
    public class BugsNotesExceptions : Exception
    {
        public BugsNotesExceptions(string message) : base(message)
        {
        }
    }

    public class BugsNotesTextRequiredException : BugsNotesExceptions
    {
        public BugsNotesTextRequiredException(string message) : base(message)
        {
        }
    }
}