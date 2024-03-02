using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Savings.Commands
{
    public class DeleteSavingCommand : ICommand
    {
        public Guid SGID { get; set; }
    }
}
