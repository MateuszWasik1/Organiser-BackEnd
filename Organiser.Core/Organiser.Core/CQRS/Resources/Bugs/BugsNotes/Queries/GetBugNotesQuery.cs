using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.CQRS.Abstraction.Queries;

namespace Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Queries
{
    public class GetBugNotesQuery : IQuery<GetBugsNotesViewModel>
    {
        public Guid BGID { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
