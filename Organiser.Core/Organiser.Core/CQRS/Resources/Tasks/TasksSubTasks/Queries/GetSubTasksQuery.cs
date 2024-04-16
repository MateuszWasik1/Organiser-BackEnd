using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries
{
    public class GetSubTasksQuery : IQuery<GetTasksSubTasksViewModel>
    {
        public Guid TGID { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
