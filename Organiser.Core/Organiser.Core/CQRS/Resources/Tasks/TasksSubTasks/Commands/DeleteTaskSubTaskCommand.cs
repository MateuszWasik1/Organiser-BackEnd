using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands
{
    public class DeleteTaskSubTaskCommand : ICommand
    {
        public Guid TSTGID { get; set; }
    }
}
