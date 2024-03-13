using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Notes.Commands
{
    public class UpdateNoteCommand : ICommand
    {
        public NotesAddViewModel Model { get; set; }
    }
}
