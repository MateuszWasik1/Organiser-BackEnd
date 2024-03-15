using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Notes.Queries
{
    public class GetNotesQuery : IQuery<List<NotesViewModel>>
    {
    }
}
