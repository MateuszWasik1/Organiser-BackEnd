using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries
{
    public class GetTaskNoteQuery : IQuery<GetTasksNotesViewModel>
    {
        public Guid TGID { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
