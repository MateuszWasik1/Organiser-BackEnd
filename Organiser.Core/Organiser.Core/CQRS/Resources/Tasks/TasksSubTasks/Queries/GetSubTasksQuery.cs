using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries
{
    public class GetSubTasksQuery : IQuery<List<TasksSubTasksViewModel>>
    {
        public Guid TGID { get; set; }
    }
}
