using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.User.Commands
{
    public class DeleteUserCommand : ICommand
    {
        public Guid UGID { get; set; }
    }
}
