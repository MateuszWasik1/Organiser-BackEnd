using Organiser.Cores.Entities;

namespace Organiser.Cores.Context
{
    public interface IDataBaseContext : IDisposable
    {
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

        void SaveChanges();
    }
}
