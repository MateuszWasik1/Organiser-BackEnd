using Organiser.Cores.Models.ViewModels.SavingsViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Savings.Commands
{
    public class AddSavingCommand : ICommand
    {
        public SavingViewModel? Model { get; set; }
    }
}
