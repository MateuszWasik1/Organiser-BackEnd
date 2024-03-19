using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries
{
    public class GetTaskQuery : IQuery<TaskViewModel>
    {
        public Guid TGID { get; set; }
    }
}
