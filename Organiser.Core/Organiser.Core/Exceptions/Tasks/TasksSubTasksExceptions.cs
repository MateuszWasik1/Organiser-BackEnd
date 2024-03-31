namespace Organiser.Core.Exceptions.Tasks
{
    public class TasksSubTasksExceptions : Exception
    {
        public TasksSubTasksExceptions(string message) : base(message)
        {
        }
    }

    public class TaskSubTaskTitleRequiredException : TasksSubTasksExceptions
    {
        public TaskSubTaskTitleRequiredException(string message) : base(message)
        {
        }
    }

    public class TaskSubTaskTitleMax200Exception : TasksSubTasksExceptions
    {
        public TaskSubTaskTitleMax200Exception(string message) : base(message)
        {
        }
    }

    public class TaskSubTaskTextRequiredException : TasksSubTasksExceptions
    {
        public TaskSubTaskTextRequiredException(string message) : base(message)
        {
        }
    }

    public class TaskSubTaskTextMax2000Exception : TasksSubTasksExceptions
    {
        public TaskSubTaskTextMax2000Exception(string message) : base(message)
        {
        }
    }
}