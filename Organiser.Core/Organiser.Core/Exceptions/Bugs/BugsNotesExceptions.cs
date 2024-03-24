namespace Organiser.Core.Exceptions.Bugs
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

    public class BugsNotesTextMax4000Exception : BugsNotesExceptions
    {
        public BugsNotesTextMax4000Exception(string message) : base(message)
        {
        }
    }
}