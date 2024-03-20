using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands
{
    public class AddTaskCommand : ICommand
    {
        public TaskViewModel? Model { get; set; }
    }
}
