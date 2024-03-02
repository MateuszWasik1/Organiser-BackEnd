using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands
{
    public class ChangeBugStatusCommand : ICommand
    {
        public ChangeBugStatusViewModel? Model { get; set; }
    }
}
