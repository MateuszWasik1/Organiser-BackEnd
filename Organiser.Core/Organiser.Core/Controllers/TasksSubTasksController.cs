using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries;
//using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.Models.ViewModels.TasksViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksSubTasksController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public TasksSubTasksController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        public List<TasksSubTasksViewModel> GetSubTasks(Guid tGID)
            => dispatcher.DispatchQuery<GetSubTasksQuery, List<TasksSubTasksViewModel>>(new GetSubTasksQuery() { TGID = tGID });

        //[HttpPost]
        //[Route("AddTaskNote")]
        //public void AddTaskNote(TasksNotesAddViewModel model)
        //    => dispatcher.DispatchCommand(new AddTaskNoteCommand() { Model = model });

        //[HttpPut]
        //[Route("ChangeSubTaskStatus")]
        //public void ChangeSubTaskStatus(TasksNotesAddViewModel model)
        //    => dispatcher.DispatchCommand(new ChangeSubTaskStatusCommand() { Model = model });

        //[HttpDelete]
        //[Route("DeleteTaskNote/{tNGID}")]
        //public void DeleteTaskNote(Guid tNGID)
        //    => dispatcher.DispatchCommand(new DeleteTaskNoteCommand() { TNGID = tNGID });
    }
}