using Organiser.Cores.Models.ViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Savings.Commands
{
    public class SaveSavingCommand : ICommand
    {
        public SavingsViewModel? Model { get; set; }
    }
}
