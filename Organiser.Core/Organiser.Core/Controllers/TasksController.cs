using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Cores.Models.ViewModels;

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
        public List<TasksViewModel> Get(string cGID = "", int status = 3)
            => dispatcher.DispatchQuery<GetTasksQuery, List<TasksViewModel>>(new GetTasksQuery() { CGID = cGID, Status = status });

        [HttpPost]
        [Route("Save")]
        public void Save(TasksViewModel model)
             => dispatcher.DispatchCommand(new SaveTaskCommand() { Model = model });

        [HttpDelete]
        [Route("Delete/{tGID}")]
        public void Delete(Guid tGID)
             => dispatcher.DispatchCommand(new DeleteTaskCommand() { TGID = tGID });
    }
}