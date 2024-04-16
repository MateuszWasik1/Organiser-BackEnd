using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Queries;
using Organiser.Core.Models.ViewModels.NotesViewModels;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public NotesController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetNote")]
        public NotesViewModel GetNote(Guid ngid)
            => dispatcher.DispatchQuery<GetNoteQuery, NotesViewModel>(new GetNoteQuery() { NGID = ngid });

        [HttpGet]
        [Route("GetNotes")]
        public GetNotesViewModel GetNotes(int skip, int take)
            => dispatcher.DispatchQuery<GetNotesQuery, GetNotesViewModel>(new GetNotesQuery() { Skip = skip, Take = take });

        [HttpPost]
        [Route("AddNote")]
        public void AddNote(NotesAddViewModel model)
            => dispatcher.DispatchCommand(new AddNoteCommand() { Model = model });
        
        [HttpPut]
        [Route("UpdateNote")]
        public void UpdateNote(NotesUpdateViewModel model)
            => dispatcher.DispatchCommand(new UpdateNoteCommand() { Model = model });

        [HttpDelete]
        [Route("DeleteNote/{ngid}")]
        public void DeleteNote(Guid ngid)
            => dispatcher.DispatchCommand(new DeleteNoteCommand() { NGID = ngid });
    }
}