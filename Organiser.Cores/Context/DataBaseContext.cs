using Microsoft.EntityFrameworkCore;
using Organiser.Cores.Entities;

namespace Organiser.Cores.Context
{
    public class DataBaseContext : IDataBaseContext
    {
        private DataContext dataContext;

        public DataBaseContext(DataContext dataContext) => this.dataContext = dataContext;

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

        public void SaveChanges() => dataContext.SaveChanges();
        public void Dispose() => dataContext.Dispose();
    }
}
