using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Commands
{
    public class SaveBugNoteCommand : ICommand
    { 
        public BugsNotesViewModel? Model { get; set; }
    }
}
