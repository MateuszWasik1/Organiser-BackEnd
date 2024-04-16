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
        public GetBugsNotesViewModel GetBugNotes(Guid bgid, int skip, int take)
            => dispatcher.DispatchQuery<GetBugNotesQuery, GetBugsNotesViewModel>(new GetBugNotesQuery() { BGID = bgid, Skip = skip, Take = take });

        [HttpPost]
        [Route("SaveBugNote")]
        public void SaveBugNote(BugsNotesViewModel model)
            => dispatcher.DispatchCommand(new SaveBugNoteCommand() { Model = model });
    }
}
