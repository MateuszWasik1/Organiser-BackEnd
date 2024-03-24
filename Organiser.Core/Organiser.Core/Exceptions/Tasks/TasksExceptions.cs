namespace Organiser.Core.Exceptions.Tasks
{
    public class TasksExceptions : Exception
    {
        public TasksExceptions(string message) : base(message)
        {
        }
    }

    public class TaskNotFoundException : TasksExceptions
    {
        public TaskNotFoundException(string message) : base(message)
        {
        }
    }

    public class TaskNameRequiredException : TasksExceptions
    {
        public TaskNameRequiredException(string message) : base(message)
        {
        }
    }

    public class TaskNameMax300Exception : TasksExceptions
    {
        public TaskNameMax300Exception(string message) : base(message)
        {
        }
    }

    public class TaskLocalizationMax300Exception : TasksExceptions
    {
        public TaskLocalizationMax300Exception(string message) : base(message)
        {
        }
    }

    public class TaskBudgetMin0Exception : TasksExceptions
    {
        public TaskBudgetMin0Exception(string message) : base(message)
        {
        }
    }
}