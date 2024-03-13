﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.Models.ViewModels.NotesViewModels;

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
        public List<NotesViewModel> GetNotes()
            => dispatcher.DispatchQuery<GetNotesQuery, List<NotesViewModel>>(new GetNotesQuery());

        [HttpPost]
        [Route("AddNote")]
        public void AddNote(NotesAddViewModel model)
            => dispatcher.DispatchCommand(new AddNoteCommand() { Model = model });
        
        [HttpPut]
        [Route("UpdateNote")]
        public void UpdateNote(NotesAddViewModel model)
            => dispatcher.DispatchCommand(new UpdateNoteCommand() { Model = model });

        [HttpPut]
        [Route("DeleteNote")]
        public void DeleteNote(Guid ngid)
            => dispatcher.DispatchCommand(new DeleteCommand() { Model = model });
    }
}