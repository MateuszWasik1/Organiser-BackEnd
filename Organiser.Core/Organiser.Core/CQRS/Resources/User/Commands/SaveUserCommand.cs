using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.User.Commands
{
    public class SaveUserCommand : ICommand
    {
        public UserViewModel? Model { get; set; }
    }
}
