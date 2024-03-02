using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands
{
    public class DeleteTaskCommand : ICommand
    {
        public Guid TGID { get; set; }
    }
}
