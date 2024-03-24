namespace Organiser.Core.Exceptions.Tasks
{
    public class TasksNotesExceptions : Exception
    {
        public TasksNotesExceptions(string message) : base(message)
        {
        }
    }

    public class TaskNotesNotFoundException : TasksNotesExceptions
    {
        public TaskNotesNotFoundException(string message) : base(message)
        {
        }
    }

    public class TaskNotesTextRequiredException : TasksNotesExceptions
    {
        public TaskNotesTextRequiredException(string message) : base(message)
        {
        }
    }

    public class TaskNotesTextMax2000Exception : TasksNotesExceptions
    {
        public TaskNotesTextMax2000Exception(string message) : base(message)
        {
        }
    }
}