using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksNotesController : ControllerBase
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public TasksNotesController(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public List<TasksNotesViewModel> Get(Guid tGID)
        {
            var task = context.Tasks.FirstOrDefault(x => x.TGID == tGID);

            if (task == null)
                throw new Exception("Nie znaleziono zadania. Nie można załadować notatek!");

            var taskNotes = context.TasksNotes.Where(x => x.TNTGID == task.TGID).OrderBy(x => x.TNDate).ToList();

            var taskNotesViewModel = new List<TasksNotesViewModel>();

            taskNotes.ForEach(x =>
            {
                taskNotesViewModel.Add(mapper.Map<TasksNotes, TasksNotesViewModel>(x));
            });

            return taskNotesViewModel;
        }

        [HttpPost]
        [Route("AddTaskNote")]
        public void AddTaskNote(TasksNotesViewModel model)
        {
            var tNote = context.TasksNotes.FirstOrDefault(x => x.TNGID == model.TNGID);
                
            if(tNote != null)
                throw new Exception("Wystąpił błąd");

            var taskNote = new TasksNotes()
            {
                TNTGID = model.TNTGID,
                TNUID = 1,
                TNNote = model.TNNote,
                TNDate = DateTime.Now,
            };

            context.CreateOrUpdate(taskNote);
            
            context.SaveChanges();
        }

        [HttpPost]
        [Route("EditTaskNote")]
        public void EditTaskNote(TasksNotesViewModel model)
        {
            var tNote = context.TasksNotes.FirstOrDefault(x => x.TNGID == model.TNGID);

            if (tNote == null)
                throw new Exception("Nie znaleziono podanej notatki! Nie została zmodyfikowana. Spróbuj ponownie.");

            var taskNote = new TasksNotes()
            {
                TNTGID = model.TNTGID,
                TNUID = 1,
                TNNote = model.TNNote,
                TNEditDate = DateTime.Now,
            };

            context.CreateOrUpdate(taskNote);

            context.SaveChanges();
        }

        [HttpDelete]
        [Route("DeleteTaskNote/{tNGID}")]
        public void DeleteTaskNote(Guid tNGID)
        {
            var taskNote = context.TasksNotes.FirstOrDefault(x => x.TNGID == tNGID);

            if(taskNote == null)
                throw new Exception("Nie znaleziono notatki do zadania! Notatka nie została usunięta.");

            context.DeleteTaskNotes(taskNote);
            context.SaveChanges();
        }
    }
}