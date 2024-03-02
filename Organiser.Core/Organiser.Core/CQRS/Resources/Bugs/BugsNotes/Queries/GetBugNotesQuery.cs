using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Queries
{
    public class GetBugNotesQuery : IQuery<List<BugsNotesViewModel>>
    {
        public Guid BGID { get; set; }
    }
}
