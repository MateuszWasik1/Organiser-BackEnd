using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;

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
        public List<TasksViewModel> Get()
        {
            //ToDo: fix where condition to accept current CUID instead of 1;
            var tasks = context.Tasks.Where(x => x.TUID == 1).ToList();

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
                    TName = model.TName,
                    TLocalization = model.TLocalization,
                    TTime = model.TTime,
                    TBudget = model.TBudget,
                };

                context.Tasks.Add(task);
            }
            else
            {
                var task = context.Tasks.FirstOrDefault(x => x.TGID == model.TGID);

                task.TName = model.TName;
                task.TLocalization = model.TLocalization;
                task.TTime = model.TTime;
                task.TBudget = model.TBudget;
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