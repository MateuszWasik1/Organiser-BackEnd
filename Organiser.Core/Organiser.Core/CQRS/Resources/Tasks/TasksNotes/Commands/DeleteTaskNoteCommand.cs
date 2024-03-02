using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands
{
    public class DeleteTaskNoteCommand : ICommand
    {
        public Guid TNGID { get; set; }
    }
}
