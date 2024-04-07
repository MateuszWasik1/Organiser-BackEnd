using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public TasksController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetTask")]
        public TaskViewModel GetTask(Guid TGID)
            => dispatcher.DispatchQuery<GetTaskQuery, TaskViewModel>(new GetTaskQuery() { TGID = TGID });

        [HttpGet]
        [Route("GetTasks")]
        public List<TasksViewModel> GetTasks(string cGID = "", int status = 3)
            => dispatcher.DispatchQuery<GetTasksQuery, List<TasksViewModel>>(new GetTasksQuery() { CGID = cGID, Status = status });

        [HttpPost]
        [Route("AddTask")]
        public void AddTask(TaskViewModel model)
             => dispatcher.DispatchCommand(new AddTaskCommand() { Model = model });

        [HttpPut]
        [Route("UpdateTask")]
        public void UpdateTask(TaskViewModel model)
            => dispatcher.DispatchCommand(new UpdateTaskCommand() { Model = model });

        [HttpDelete]
        [Route("Delete/{tGID}")]
        public void Delete(Guid tGID)
             => dispatcher.DispatchCommand(new DeleteTaskCommand() { TGID = tGID });

        [HttpPost]
        [Route("DeleteWithRelatedEntities")]
        public void DeleteWithRelatedEntities(TasksDeleteTaskRelatedEntitiesViewModel model)
            => dispatcher.DispatchCommand(new DeleteTaskRelatedEntitiesCommand() { Model = model });
    }
}