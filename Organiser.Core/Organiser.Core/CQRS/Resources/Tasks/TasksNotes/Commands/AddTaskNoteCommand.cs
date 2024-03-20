using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands
{
    public class AddTaskNoteCommand : ICommand
    {
        public TasksNotesAddViewModel? Model { get; set; }
    }
}
