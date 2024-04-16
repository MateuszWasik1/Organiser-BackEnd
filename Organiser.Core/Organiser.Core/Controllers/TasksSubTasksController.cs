using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries;
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
        [Route("GetSubTasks")]
        public GetTasksSubTasksViewModel GetSubTasks(Guid tGID, int skip, int take)
            => dispatcher.DispatchQuery<GetSubTasksQuery, GetTasksSubTasksViewModel>(new GetSubTasksQuery() { TGID = tGID, Skip = skip, Take = take });

        [HttpPost]
        [Route("AddTaskSubTask")]
        public void AddTaskSubTask(TasksAddSubTaskViewModel model)
            => dispatcher.DispatchCommand(new AddTaskSubTaskCommand() { Model = model });

        [HttpPut]
        [Route("ChangeSubTaskStatus")]
        public void ChangeSubTaskStatus(TasksSubTasksChangeStatusViewModel model)
            => dispatcher.DispatchCommand(new ChangeTaskSubTaskStatusCommand() { Model = model });

        [HttpDelete]
        [Route("DeleteTaskSubTask/{tstGID}")]
        public void DeleteTaskSubTask(Guid tstGID)
            => dispatcher.DispatchCommand(new DeleteTaskSubTaskCommand() { TSTGID = tstGID });
    }
}