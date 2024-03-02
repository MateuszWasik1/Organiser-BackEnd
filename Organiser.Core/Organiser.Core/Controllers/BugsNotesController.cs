using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Commands;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BugsNotesController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public BugsNotesController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetBugNotes")]
        public List<BugsNotesViewModel> GetBugNotes(Guid bgid)
            => dispatcher.DispatchQuery<GetBugNotesQuery, List<BugsNotesViewModel>>(new GetBugNotesQuery() { BGID = bgid });

        [HttpPost]
        [Route("SaveBugNote")]
        public void SaveBugNote(BugsNotesViewModel model)
            => dispatcher.DispatchCommand(new SaveBugNoteCommand() { Model = model });
    }
}
