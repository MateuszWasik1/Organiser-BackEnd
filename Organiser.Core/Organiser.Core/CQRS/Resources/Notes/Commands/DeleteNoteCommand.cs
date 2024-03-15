using Organiser.CQRS.Abstraction.Commands;

namespace Organiser.Core.CQRS.Resources.Notes.Commands
{
    public class DeleteNoteCommand : ICommand
    {
        public Guid NGID { get; set; }
    }
}
