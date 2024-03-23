namespace Organiser.Core.Exceptions.Categories
{
    public class CategoriesExceptions : Exception
    {
        public CategoriesExceptions(string message) : base(message)
        {
        }
    }

    public class CategoryNotFoundException : CategoriesExceptions
    {
        public CategoryNotFoundException(string message) : base(message)
        {
        }
    }

    public class CategoryNameRequiredException : CategoriesExceptions
    {
        public CategoryNameRequiredException(string message) : base(message)
        {
        }
    }

    public class CategoryNameMax300Exception : CategoriesExceptions
    {
        public CategoryNameMax300Exception(string message) : base(message)
        {
        }
    }

    public class CategoryBudgetMin0Exception : CategoriesExceptions
    {
        public CategoryBudgetMin0Exception(string message) : base(message)
        {
        }
    }

    public class CategoryHasTasksException : CategoriesExceptions
    {
        public CategoryHasTasksException(string message) : base(message)
        {
        }
    }
}