using Microsoft.EntityFrameworkCore;
using Organiser.Cores.Entities;
using Organiser.Cores.Services;

namespace Organiser.Cores.Context
{
    public class DataBaseContext : IDataBaseContext
    {
        private DataContext dataContext;
        private IUserContext user;

        public DataBaseContext(DataContext dataContext, IUserContext user)
        {
            this.dataContext = dataContext;
            this.user = user;
        }

        #region User
        public IQueryable<User> User => dataContext.User;
        public void CreateOrUpdate(User user)
        {
            if (user.UID == default)
                dataContext.User.Add(user);
            else
                dataContext.Entry(user).State = EntityState.Modified;
        }
        public void DeleteUser(User user) => dataContext.User.Remove(user);
        #endregion

        #region Roles
        public IQueryable<Roles> Roles => dataContext.AppRoles;
        #endregion

        #region Categories
        public IQueryable<Categories> Categories => dataContext.Categories.Where(x => x.CUID == user.UID);
        public void CreateOrUpdate(Categories category)
        {
            if (category.CID == default)
                dataContext.Categories.Add(category);
            else
                dataContext.Entry(category).State = EntityState.Modified;
        }
        public void DeleteCategory(Categories category) => dataContext.Categories.Remove(category);
        #endregion

        #region Tasks
        public IQueryable<Tasks> Tasks => dataContext.Tasks.Where(x => x.TUID == user.UID);
        public void CreateOrUpdate(Tasks task)
        {
            if (task.TID == default)
                dataContext.Tasks.Add(task);
            else
                dataContext.Entry(task).State = EntityState.Modified;
        }
        public void DeleteTask(Tasks task) => dataContext.Tasks.Remove(task);
        #endregion

        #region TasksNotes
        public IQueryable<TasksNotes> TasksNotes => dataContext.TasksNotes.Where(x => x.TNUID == user.UID);
        public void CreateOrUpdate(TasksNotes taskNotes)
        {
            if (taskNotes.TNID == default)
                dataContext.TasksNotes.Add(taskNotes);
            else
                dataContext.Entry(taskNotes).State = EntityState.Modified;
        }
        public void DeleteTaskNotes(TasksNotes taskNotes) => dataContext.TasksNotes.Remove(taskNotes);
        #endregion

        #region Savings
        public IQueryable<Savings> Savings => dataContext.Savings.Where(x => x.SUID == user.UID);
        public void CreateOrUpdate(Savings saving)
        {
            if (saving.SID == default)
                dataContext.Savings.Add(saving);
            else
                dataContext.Entry(saving).State = EntityState.Modified;
        }
        public void DeleteSaving(Savings saving) => dataContext.Savings.Remove(saving);
        #endregion

        #region Users
        public IQueryable<User> AllUsers => dataContext.User;
        public IQueryable<Categories> AllCategories => dataContext.Categories;
        public IQueryable<Tasks> AllTasks => dataContext.Tasks;
        public IQueryable<TasksNotes> AllTasksNotes => dataContext.TasksNotes;
        public IQueryable<Savings> AllSavings => dataContext.Savings;
        #endregion

        #region Bugs
        public IQueryable<Bugs> Bugs => dataContext.Bugs.Where(x => x.BUID == user.UID);
        public IQueryable<Bugs> AllBugs => dataContext.Bugs;
        public void CreateOrUpdate(Bugs bug)
        {
            if (bug.BID == default)
                dataContext.Bugs.Add(bug);
            else
                dataContext.Entry(bug).State = EntityState.Modified;
        }
        #endregion

        #region BugsNotes
        public IQueryable<BugsNotes> BugsNotes => dataContext.BugsNotes.Where(x => x.BNUID == user.UID);
        public IQueryable<BugsNotes> AllBugsNotes => dataContext.BugsNotes;
        public void CreateOrUpdate(BugsNotes bugNote)
        {
            if (bugNote.BNID == default)
                dataContext.BugsNotes.Add(bugNote);
            else
                dataContext.Entry(bugNote).State = EntityState.Modified;
        }
        public void DeleteBugNote(BugsNotes bugNote) => dataContext.BugsNotes.Remove(bugNote);
        #endregion

        #region Notes
        public IQueryable<Notes> Notes => dataContext.Notes.Where(x => x.NUID == user.UID);
        public IQueryable<Notes> AllNotes => dataContext.Notes;
        public void CreateOrUpdate(Notes note)
        {
            if (note.NID == default)
                dataContext.Notes.Add(note);
            else
                dataContext.Entry(note).State = EntityState.Modified;
        }
        public void DeleteNote(Notes note) => dataContext.Notes.Remove(note);
        #endregion

        public void SaveChanges() => dataContext.SaveChanges();
        public void Dispose() => dataContext.Dispose();
    }
}
