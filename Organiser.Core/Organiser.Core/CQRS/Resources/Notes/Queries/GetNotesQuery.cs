using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Notes.Queries
{
    public class GetNotesQuery : IQuery<GetNotesViewModel>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
