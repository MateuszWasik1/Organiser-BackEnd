namespace Organiser.Core.Exceptions.Tasks
{
    public class UsersExceptions : Exception
    {
        public UsersExceptions(string message) : base(message)
        {
        }
    }

    public class UserNameRequiredException : UsersExceptions
    {
        public UserNameRequiredException(string message) : base(message)
        {
        }
    }

    public class UserNameMax100Exception : TasksNotesExceptions
    {
        public UserNameMax100Exception(string message) : base(message)
        {
        }
    }

    public class UserFirstNameMax50Exception : TasksNotesExceptions
    {
        public UserFirstNameMax50Exception(string message) : base(message)
        {
        }
    }

    public class UserLastNameMax50Exception : TasksNotesExceptions
    {
        public UserLastNameMax50Exception(string message) : base(message)
        {
        }
    }

    public class UserPhoneMax100Exception : TasksNotesExceptions
    {
        public UserPhoneMax100Exception(string message) : base(message)
        {
        }
    }

    public class UserEmailRequiredException : TasksNotesExceptions
    {
        public UserEmailRequiredException(string message) : base(message)
        {
        }
    }

    public class UserEmailMax100Exception : TasksNotesExceptions
    {
        public UserEmailMax100Exception(string message) : base(message)
        {
        }
    }
}