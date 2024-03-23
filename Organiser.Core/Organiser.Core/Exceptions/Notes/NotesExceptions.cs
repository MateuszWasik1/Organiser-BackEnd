namespace Organiser.Core.Exceptions.Notes
{
    public class NotesExceptions : Exception
    {
        public NotesExceptions(string message) : base(message)
        {
        }
    }

    public class NoteNotFoundException : NotesExceptions
    {
        public NoteNotFoundException(string message) : base(message)
        {
        }
    }

    public class NoteTitleRequiredException : NotesExceptions
    {
        public NoteTitleRequiredException(string message) : base(message)
        {
        }
    }

    public class NoteTitleMax200Exception : NotesExceptions
    {
        public NoteTitleMax200Exception(string message) : base(message)
        {
        }
    }

    public class NoteTextRequiredException : NotesExceptions
    {
        public NoteTextRequiredException(string message) : base(message)
        {
        }
    }

    public class NoteTitleMax4000Exception : NotesExceptions
    {
        public NoteTitleMax4000Exception(string message) : base(message)
        {
        }
    }
}