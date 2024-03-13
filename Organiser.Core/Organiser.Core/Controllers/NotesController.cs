using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;

namespace Organiser.Cores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly IDispatcher dispatcher;
        public NotesController(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        [HttpGet]
        [Route("GetNotes")]
        public string GetNotes()
            => dispatcher.DispatchQuery<GetNotesQuery, string>(new GetNotesQuery());

        [HttpPost]
        [Route("AddNote")]
        public void AddNote(NoteViewModel model)
            => dispatcher.DispatchCommand(new AddNoteCommand() { Model = model });
        
        [HttpPut]
        [Route("UpdateNote")]
        public void UpdateNote(NoteViewModel model)
            => dispatcher.DispatchCommand(new UpdateNoteCommand() { Model = model });

        [HttpPut]
        [Route("DeleteNote")]
        public void DeleteNote(Guid ngid)
            => dispatcher.DispatchCommand(new DeleteCommand() { Model = model });
    }
}