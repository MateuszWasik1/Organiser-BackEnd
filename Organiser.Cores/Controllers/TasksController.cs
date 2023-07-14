using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;
using System.Threading.Tasks;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public TasksController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public List<TasksViewModel> Get(string cGID = "", int status = 0)
        {
            //ToDo: fix where condition to accept current CUID instead of 1;
            var tasks = context.Tasks.Where(x => x.TUID == 1).OrderBy(x => x.TTime).ToList();

            if(!string.IsNullOrEmpty(cGID))
                tasks = tasks.Where(x => x.TCGID == Guid.Parse(cGID)).ToList();

            if(status != 0)
                tasks = tasks.Where(x => (int) x.TStatus == status).ToList();

            var tasksViewModel = new List<TasksViewModel>();

            tasks.ForEach(x =>
            {
                tasksViewModel.Add(mapper.Map<Tasks, TasksViewModel>(x));
            });

            return tasksViewModel;
        }

        [HttpPost]
        [Route("Save")]
        public void Save(TasksViewModel model)
        {
            if (model.TID == 0)
            {
                var task = new Tasks()
                {
                    TGID = model.TGID,
                    TUID = 1, //poprawić by przekazywało poprawny UID
                    TCGID = model.TCGID,
                    TName = model.TName,
                    TLocalization = model.TLocalization,
                    TTime = model.TTime,
                    TBudget = model.TBudget,
                    TStatus = model.TStatus,
                };

                context.Tasks.Add(task);
            }
            else
            {
                var task = context.Tasks.FirstOrDefault(x => x.TGID == model.TGID);

                task.TCGID = model.TCGID;
                task.TName = model.TName;
                task.TLocalization = model.TLocalization;
                task.TTime = model.TTime;
                task.TBudget = model.TBudget;
                task.TStatus = model.TStatus;
            }

            context.SaveChanges();
        }

        [HttpDelete]
        [Route("Delete/{tGID}")]
        public void Delete(Guid tGID)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == tGID);

            context.Remove(task);
            context.SaveChanges();
        }
    }
}