using Organiser.Cores.Entities;

namespace Organiser.Cores.Context
{
    public interface IDataBaseContext : IDisposable
    {
        #region User
        IQueryable<User> User { get; }
        void CreateOrUpdate(User user);
        void DeleteUser(User user);
        #endregion

        #region Roles
        IQueryable<Roles> Roles { get; }
        #endregion

        #region Categories
        IQueryable<Categories> Categories { get; }
        void CreateOrUpdate(Categories category);
        void DeleteCategory(Categories category);
        #endregion

        #region Tasks
        IQueryable<Tasks> Tasks { get; }
        void CreateOrUpdate(Tasks task);
        void DeleteTask(Tasks task);
        #endregion

        #region TasksNotes
        IQueryable<TasksNotes> TasksNotes { get; }
        void CreateOrUpdate(TasksNotes taskNotes);
        void DeleteTaskNotes(TasksNotes taskNotes);
        #endregion

        #region Savings
        IQueryable<Savings> Savings { get; }
        void CreateOrUpdate(Savings saving);
        void DeleteSaving(Savings saving);
        #endregion

        #region Users
        IQueryable<User> AllUsers { get; }
        IQueryable<Categories> AllCategories { get; }
        IQueryable<Tasks> AllTasks { get; }
        IQueryable<TasksNotes> AllTasksNotes { get; }
        IQueryable<Savings> AllSavings { get; }
        #endregion

        void SaveChanges();
    }
}
