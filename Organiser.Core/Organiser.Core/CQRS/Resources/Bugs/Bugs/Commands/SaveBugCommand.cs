using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands
{
    public class SaveBugCommand : ICommand
    {
        public BugViewModel? Model { get; set; }
    }
}
