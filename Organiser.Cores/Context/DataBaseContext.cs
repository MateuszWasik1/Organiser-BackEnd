using Microsoft.EntityFrameworkCore;
using Organiser.Cores.Entities;

namespace Organiser.Cores.Context
{
    public class DataBaseContext : IDataBaseContext
    {
        private DataContext dataContext;

        public DataBaseContext(DataContext dataContext) => this.dataContext = dataContext;

        #region Users
        public IQueryable<Users> Users => dataContext.Users;
        public void CreateOrUpdate(Users user)
        {
            if (user.UID == default)
                dataContext.Users.Add(user);
            else
                dataContext.Entry(user).State = EntityState.Modified;
        }
        public void DeleteUser(Users user) => dataContext.Users.Remove(user);
        #endregion

        #region Roles
        public IQueryable<Roles> Roles => dataContext.Roles;
        #endregion

        #region Categories
        public IQueryable<Categories> Categories => dataContext.Categories.Where(x => x.CUID == 1);
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
        public IQueryable<Tasks> Tasks => dataContext.Tasks.Where(x => x.TUID == 1);
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
        public IQueryable<TasksNotes> TasksNotes => dataContext.TasksNotes.Where(x => x.TNUID == 1);
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
        public IQueryable<Savings> Savings => dataContext.Savings.Where(x => x.SUID == 1);
        public void CreateOrUpdate(Savings saving)
        {
            if (saving.SID == default)
                dataContext.Savings.Add(saving);
            else
                dataContext.Entry(saving).State = EntityState.Modified;
        }
        public void DeleteSaving(Savings saving) => dataContext.Savings.Remove(saving);
        #endregion

        public void SaveChanges() => dataContext.SaveChanges();
        public void Dispose() => dataContext.Dispose();
    }
}
