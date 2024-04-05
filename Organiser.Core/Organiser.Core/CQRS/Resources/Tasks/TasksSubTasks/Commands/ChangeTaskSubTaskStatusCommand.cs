using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands
{
    public class ChangeTaskSubTaskStatusCommand : ICommand
    {
        public TasksSubTasksChangeStatusViewModel? Model { get; set; }
    }
}
