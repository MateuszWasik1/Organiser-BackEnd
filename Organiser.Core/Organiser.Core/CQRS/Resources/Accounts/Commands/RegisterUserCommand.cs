using Organiser.Cores.Models.ViewModels.AccountsViewModel;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Accounts.Commands
{
    public class RegisterUserCommand : ICommand
    {
        public RegisterViewModel? Model { get; set; }
    }
}
