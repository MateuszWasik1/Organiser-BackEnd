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

        #region TasksSubTasks
        IQueryable<TasksSubTasks> TasksSubTasks { get; }
        void CreateOrUpdate(TasksSubTasks TaskSubTask);
        void DeleteTaskSubTask(TasksSubTasks TaskSubTask);
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
        IQueryable<TasksSubTasks> AllTasksSubTasks { get; }
        IQueryable<Savings> AllSavings { get; }
        #endregion

        #region Bugs
        IQueryable<Bugs> Bugs { get; }
        IQueryable<Bugs> AllBugs { get; }
        void CreateOrUpdate(Bugs bug);
        #endregion

        #region BugsNotes
        IQueryable<BugsNotes> BugsNotes { get; }
        IQueryable<BugsNotes> AllBugsNotes { get; }
        void CreateOrUpdate(BugsNotes bugNote);
        void DeleteBugNote(BugsNotes bugNote);
        #endregion

        #region Notes
        IQueryable<Notes> Notes { get; }
        IQueryable<Notes> AllNotes { get; }
        void CreateOrUpdate(Notes note);
        void DeleteNote(Notes note);
        #endregion

        void SaveChanges();
    }
}
