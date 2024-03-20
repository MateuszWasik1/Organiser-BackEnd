using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries
{
    public class GetTaskNoteQuery : IQuery<List<TasksNotesViewModel>>
    {
        public Guid TGID { get; set; }
    }
}
