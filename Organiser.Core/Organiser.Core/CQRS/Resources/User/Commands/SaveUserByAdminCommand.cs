using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.User.Commands
{
    public class SaveUserByAdminCommand : ICommand
    {
        public UserAdminViewModel? Model { get; set; }
    }
}
