using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Notes.Queries
{
    public class GetNoteQuery : IQuery<NotesViewModel>
    {
        public Guid NGID { get; set; }
    }
}