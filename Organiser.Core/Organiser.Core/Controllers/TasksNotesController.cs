using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksNotesController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public TasksNotesController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        public GetTasksNotesViewModel Get(Guid tGID, int skip, int take)
            => dispatcher.DispatchQuery<GetTaskNoteQuery, GetTasksNotesViewModel>(new GetTaskNoteQuery() { TGID = tGID, Skip = skip, Take = take });

        [HttpPost]
        [Route("AddTaskNote")]
        public void AddTaskNote(TasksNotesAddViewModel model)
            => dispatcher.DispatchCommand(new AddTaskNoteCommand() { Model = model });

        [HttpDelete]
        [Route("DeleteTaskNote/{tNGID}")]
        public void DeleteTaskNote(Guid tNGID)
            => dispatcher.DispatchCommand(new DeleteTaskNoteCommand() { TNGID = tNGID });
    }
}