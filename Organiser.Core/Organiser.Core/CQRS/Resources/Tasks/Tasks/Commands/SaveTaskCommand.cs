using Organiser.Cores.Models.ViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands
{
    public class SaveTaskCommand : ICommand
    {
        public TasksViewModel? Model { get; set; }
    }
}
