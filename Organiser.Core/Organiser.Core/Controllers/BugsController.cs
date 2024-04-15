using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Models.Enums;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BugsController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public BugsController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetBug")]
        public BugViewModel GetBug(Guid bgid)
            => dispatcher.DispatchQuery<GetBugQuery, BugViewModel>(new GetBugQuery() { BGID = bgid });

        [HttpGet]
        [Route("GetBugs")]
        public GetBugsViewModel GetBugs(BugTypeEnum bugType)
            => dispatcher.DispatchQuery<GetBugsQuery, GetBugsViewModel>(new GetBugsQuery() { BugType = bugType });

        [HttpPost]
        [Route("SaveBug")]
        public void SaveBug(BugViewModel model)
            => dispatcher.DispatchCommand(new SaveBugCommand() { Model = model });

        [HttpPost]
        [Route("ChangeBugStatus")]
        public void ChangeBugStatus(ChangeBugStatusViewModel model)
            => dispatcher.DispatchCommand(new ChangeBugStatusCommand() { Model = model });
    }
}
